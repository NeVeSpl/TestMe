using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TestMe.Presentation.API.Configurations
{
    internal static class OpenAPI
    {
        private const string Version = "v1.0";


        public static void AddOpenAPI(this IServiceCollection services)
        {
            services.AddSwaggerGen(setup =>
            {                
                setup.SwaggerDoc(Version, new OpenApiInfo() 
                { 
                    Title = "TestMe.API",
                    Description = "WebAPI that plays the role of api gateway to modular monolith behind",                   
                });
                setup.CustomSchemaIds(x => x.FullName);
                
                var xmlDocumentationFilePath = Path.Combine(System.AppContext.BaseDirectory, "TestMe.Presentation.API.xml");
                setup.IncludeXmlComments(xmlDocumentationFilePath);

                setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Scheme = "bearer",  
                    In = ParameterLocation.Header
                });
               
                setup.OperationFilter<AuthorizeCheckOperationFilter>();
            });
        }

        public static void UseOpenAPI(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(setup =>
            {
                setup.SwaggerEndpoint($"/swagger/{Version}/swagger.json", "TestMe.API");
            });
        }


        internal class AuthorizeCheckOperationFilter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                context.ApiDescription.TryGetMethodInfo(out var methodInfo);

                if (methodInfo == null)
                    return;

                var hasAuthorizeAttribute = false;

                if ((methodInfo.MemberType == MemberTypes.Method) && (methodInfo.DeclaringType != null))
                {
                    // NOTE: Check the controller itself has Authorize attribute
                    hasAuthorizeAttribute = methodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

                    // NOTE: Controller has Authorize attribute, so check the endpoint itself.
                    //       Take into account the allow anonymous attribute
                    if (hasAuthorizeAttribute)
                        hasAuthorizeAttribute = !methodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any();
                    else
                        hasAuthorizeAttribute = methodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();
                }

                if (!hasAuthorizeAttribute)
                    return;

                var scheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                };
                // NOTE: This adds the "Padlock" icon to the endpoint in swagger, 
                //       we can also pass through the names of the policies in the string[]
                //       which will indicate which permission you require.
                operation.Security = new List<OpenApiSecurityRequirement>()
                {
                    new OpenApiSecurityRequirement
                    {
                        [scheme] = new string[]{ }
                    }
                };            
            }
        }
    }
}
