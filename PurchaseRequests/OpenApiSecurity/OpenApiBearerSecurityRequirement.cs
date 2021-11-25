using System;
using Microsoft.OpenApi.Models;

namespace ProductsCRUD.OpenApiSecurity
{
    public class OpenApiBearerSecurityRequirement : OpenApiSecurityRequirement
    {
        public OpenApiBearerSecurityRequirement(OpenApiSecurityScheme securityScheme)
        {
            this.Add(securityScheme, new[] { "Bearer" });
        }
    }
}