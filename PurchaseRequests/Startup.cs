using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Reflection;
using System.IO;
using ProductsCRUD.OpenApiSecurity;
using Microsoft.IdentityModel.Logging;
using PurchaseRequests.Repositories.Interface;
using PurchaseRequests.Repositories.Concrete;
using PurchaseRequests.AutomatedCacher.Interface;
using ProductsCRUD.AutomatedCacher.Concrete;
using PurchaseRequests.AutomatedCacher.Model;
using PurchaseRequests.CustomExceptionHandler;

namespace PurchaseRequests
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;

            // Dependency injection for using the azure database when in staging or production.
            // The connection string is stored in the azure web app and injected from there to the appsettings JSON.
            if (_environment.IsDevelopment())
            {
                services.AddDbContext<Context.Context>(options => options.UseSqlServer
                    ("local"));
            } else
            {
                services.AddDbContext<Context.Context>(options => options.UseSqlServer
                    (Configuration.GetConnectionString("PurchaseRequestsConnectionString"),
                    sqlServerOptionsAction: sqlOptions => sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(2),
                    errorNumbersToAdd: null)));
            }

            // Stack used for patch requests.
            services.AddControllers().AddNewtonsoftJson(j =>
            {
                j.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            // Automapper for mapping DTOs to domain models.
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Setting up authentication with Auth0 using configuration from the appsettings JSON.
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = $"https://{Configuration["Auth0:Domain"]}/";
                options.Audience = Configuration["Auth0:Audience"];
            });

            // Using the fake repository while in development, otherwise using the real one.
            if (_environment.IsDevelopment())
            {
                services.AddSingleton<IPurchaseRequestsRepository, FakePurchaseRequestsRepository>();
            }
            else
            {
                services.AddScoped<IPurchaseRequestsRepository, SqlPurchaseRequestsRepository>();
            }

            // Configuration of swagger for ease of development. Authentication for swagger is also configured here.
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "PurchaseRequests API",
                    Description = "An ASP.NET Core Web API for managing the purchase requests of ThAmCo."
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

                string securityDefinitionName = Configuration["SwaggerUISecurityMode"] ?? "Bearer";
                OpenApiSecurityScheme securityScheme = new OpenApiBearerSecurityScheme();
                OpenApiSecurityRequirement securityRequirement = new OpenApiBearerSecurityRequirement(securityScheme);

                if (securityDefinitionName.ToLower() == "oauth2")
                {
                    securityScheme = new OpenApiOAuthSecurityScheme(Configuration["Auth0:Domain"], Configuration["Auth0:Audience"]);
                    securityRequirement = new OpenApiOAuthSecurityRequirement();
                }

                options.AddSecurityDefinition(securityDefinitionName, securityScheme);

                options.AddSecurityRequirement(securityRequirement);
            });

            // Setting the permissions that will be used on endpoints and mapping them to an alias that we will use.
            services.AddAuthorization(o =>
            {
                o.AddPolicy("ReadPurchaseRequests", policy =>
                    policy.RequireClaim("permissions", "read:purchase-requests"));
                o.AddPolicy("ReadPurchaseRequest", policy =>
                    policy.RequireClaim("permissions", "read:purchase-request"));
                o.AddPolicy("CreatePurchaseRequest", policy =>
                    policy.RequireClaim("permissions", "add:purchase-request"));
                o.AddPolicy("UpdatePurchaseRequest", policy =>
                    policy.RequireClaim("permissions", "edit:purchase-request"));
                o.AddPolicy("ReadPendingPurchaseRequests", policy =>
                    policy.RequireClaim("permissions", "read:pending-purchase-requests"));
            });

            services.AddMvc(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
            });

            // Configuration of the automated cacher.
            services.AddSingleton<IMemoryCacheAutomater, MemoryCacheAutomater>();
            services.Configure<MemoryCacheModel>(Configuration.GetSection("MemoryCache"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMemoryCacheAutomater memoryCacheAutomater, Context.Context dataContext)
        {
            if (env.IsDevelopment())
            {
                // In development, we use Swagger for ease of testing.
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Glossary v1");

                    if (Configuration["SwaggerUISecurityMode"]?.ToLower() == "oauth2")
                    {
                        c.OAuthClientId(Configuration["Auth0:ClientId"]);
                        c.OAuthClientSecret(Configuration["Auth0:ClientSecret"]);
                        c.OAuthAppName("GlossaryClient");
                        c.OAuthAdditionalQueryStringParams(new Dictionary<string, string> { { "audience", Configuration["Auth0:Audience"] } });
                        c.OAuthUsePkce();
                    }
                });
            } else if (env.IsStaging())
            {
                // For Staging, we use the custom exception middleware, we migrate the database in case of any new migrations and we activate the automated cacher.
                // We also use the Swagger UI for ease of access, since staging is not customer facing.
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Glossary v1");

                    if (Configuration["SwaggerUISecurityMode"]?.ToLower() == "oauth2")
                    {
                        c.OAuthClientId(Configuration["Auth0:ClientId"]);
                        c.OAuthClientSecret(Configuration["Auth0:ClientSecret"]);
                        c.OAuthAppName("GlossaryClient");
                        c.OAuthAdditionalQueryStringParams(new Dictionary<string, string> { { "audience", Configuration["Auth0:Audience"] } });
                        c.OAuthUsePkce();
                    }
                });
                dataContext.Database.Migrate();
                app.UseMiddleware<ExceptionMiddleware>();
                memoryCacheAutomater.AutomateCache();
            }
            else if (env.IsProduction())
            {
                // For Production, we use the custom exception middleware, we migrate the database in case of any new migrations and we activate the automated cacher
                dataContext.Database.Migrate();
                app.UseMiddleware<ExceptionMiddleware>();
                memoryCacheAutomater.AutomateCache();
            }
            else
            {
                // For an unexpected stage, we only use the exception middleware and migrate the database.
                dataContext.Database.Migrate();
                app.UseMiddleware<ExceptionMiddleware>();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
        }
    }
}
