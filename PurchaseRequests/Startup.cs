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

            if(_environment.IsDevelopment())
            {
                services.AddDbContext<Context.Context>(options => options.UseSqlServer
                    ("local"));
            } else
            {
                services.AddDbContext<Context.Context>(options => options.UseSqlServer
                    (Configuration.GetConnectionString("PurchaseRequestsConnectionString")));
            }

            services.AddControllers().AddNewtonsoftJson(j =>
            {
                j.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = $"https://{Configuration["Auth0:Domain"]}/";
                options.Audience = Configuration["Auth0:Audience"];
            });

            //Using the fake repository while in development
            if(_environment.IsDevelopment())
            {
                services.AddSingleton<IPurchaseRequestsRepository, FakePurchaseRequestsRepository>();
            }
            else
            {
                services.AddSingleton<IPurchaseRequestsRepository, SqlPurchaseRequestsRepository>();
            }

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

            

            services.AddMvc(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
            });

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Context.Context dataContext)
        {
            if (env.IsDevelopment())
            {
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
                
            } else
            {
                dataContext.Database.Migrate();
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
