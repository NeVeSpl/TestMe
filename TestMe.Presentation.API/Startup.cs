using System;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using TestMe.BuildingBlocks.EventBus;
using TestMe.Infrastructure.EventBus.InMemory;
using TestMe.Infrastructure.EventBus.RabbitMQ;
using TestMe.Presentation.API.BackgroundServices;
using TestMe.Presentation.API.Configurations;
using TestMe.Presentation.API.Middleware;
using TestMe.Presentation.API.Services;
using TestMe.TestCreation.App;
using TestMe.TestCreation.Infrastructure;
using TestMe.TestCreation.Persistence;
using TestMe.UserManagement.App;
using TestMe.UserManagement.Infrastructure;
using TestMe.UserManagement.Persistence.Extensions;

namespace TestMe.Presentation.API
{
    public class Startup
    {
        private IConfiguration Configuration { get; }       


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;           
        }


        public void ConfigureServices(IServiceCollection services)
        {
            var config = new StartupConfig();
            Configuration.Bind("StartupConfiguration", config);

            services.AddTestCreationPersistence(Configuration.GetConnectionString("TestCreationDbContext"));
            services.AddTestCreationApplicationServices();
            services.AddTestCreationInfrastructureServices();

            services.AddUserManagementPersistence(Configuration.GetConnectionString("UserManagementDbContext"));
            services.AddUserManagementApplicationServices();
            services.AddUserManagementInfrastructureServices();

            services.AddMediatR(typeof(TestCreation.App.IServiceCollectionExtensionsFromTestCreationApp));

            services.Configure<AuthenticationService.Config>(Configuration.GetSection("Jwt"));
            services.AddJWTAuthentication(Configuration, "Jwt"); 
            services.AddControllers();
            services.AddCORS();
            services.AddOpenAPI();
            services.AddResponseCaching();            
            services.AddHealthChecks();

            services.Configure<PostManService.Config>(Configuration.GetSection("PostManService"));
            services.AddHostedService<PostManService>();

            services.Configure<RabbitMQEventBus.Config>(Configuration.GetSection("RabbitMQ"));
            switch (config.EventBus)
            {
                case StartupConfig.EventBusType.InMemory:
                    services.AddSingleton<IEventBus, InMemoryEventBus>();
                    break;
                case StartupConfig.EventBusType.RabbitMQ:
                    services.AddSingleton<IEventBus, RabbitMQEventBus>();
                    break;
                default:
                    throw new NotImplementedException();
            } 

            services.AddHttpContextAccessor();
            services.AddPresentationAPIServices();
        }   
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.SubscribeTestCreationEventHandlers();

            //app.UseSerilogRequestLogging();
            app.UseRequestLogger();
            app.UseOpenAPI();

            if (env.IsDevelopment())
            {                
                //app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
                app.UseExceptionHandler("/Error");
                IdentityModelEventSource.ShowPII = true;
            }
            else
            {
                app.UseExceptionHandler("/Error");                
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseRouting();      
            app.UseResponseCaching();
            app.UseCORS(); 
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });          
        }

        private class StartupConfig
        {
            internal enum EventBusType { InMemory, RabbitMQ }

            public EventBusType EventBus { get; set; }             
        }
    }
}