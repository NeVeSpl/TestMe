using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace TestMe.Presentation.API.Configurations
{
    internal static class CORS
    {
        private const string PolicyName = "CorsPolicy";

        public static void AddCORS(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(PolicyName,
                    builder => builder.WithOrigins("https://localhost:44362", "https://testme.mw-neves.pl")
                                      .AllowAnyMethod()
                                      .AllowAnyHeader()
                                      .AllowCredentials()
                .Build());
            });
        }

        public static void UseCORS(this IApplicationBuilder app)
        {
            app.UseCors(PolicyName);
        }
    }
}
