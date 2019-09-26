using Microsoft.AspNetCore.Builder;

namespace TestMe.Presentation.API.Configurations
{
    internal static class NSwag
    {
        public static void UseNSwag(this IApplicationBuilder app)
        {
            app.Map("/swagger", x =>
            {
                app.UseStaticFiles();
                //app.UseSwagger();
                //app.UseSwaggerUi3();
            });
        }
    }
}
