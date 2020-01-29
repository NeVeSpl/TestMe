using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using TestMe.BuildingBlocks.EventBus;
using TestMe.UserManagement.Infrastructure.AutoMapper;
using TestMe.UserManagement.Infrastructure.EventBus;

namespace TestMe.UserManagement.Infrastructure
{
    public static class IServiceCollectionExtensions
    {
        public static void AddUserManagementInfrastructureServices(this IServiceCollection services)
        {  
            services.AddTransient<IEventSender, EventSender>();
            /*
             *  AutoMapper is used in this module only to remind how painful is maintaining it over time.
             *  Do not use AutoMapper in real app, use  cezarypiatek/MappingGenerator instead.
             *  AutoMapper is considered harmful.
             *  AutoMapper is anti-pattern.
             *  AutoMapper is the worst thing that you can do to your project.               
             */
            services.AddAutoMapper(typeof(UserProfile));
        }
    }
}
