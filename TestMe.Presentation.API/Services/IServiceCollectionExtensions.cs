using Microsoft.Extensions.DependencyInjection;
using TestMe.TestCreation.App;
using TestMe.UserManagement.App;

namespace TestMe.Presentation.API.Services
{
    public static class IServiceCollectionExtensions
    {
        public static void AddPresentationAPIServices(this IServiceCollection services)
        {
            services.AddScoped<ITraceIdProvider, TraceIdProvider>();
            services.AddScoped<IUserIdProvider, UserIdProvider>();
        }
    }
}
