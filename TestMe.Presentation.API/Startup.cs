using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using TestMe.BuildingBlocks.EventBus;
using TestMe.Infrastructure.EventBus.InMemory;
using TestMe.Presentation.API.BackgroundServices;
using TestMe.Presentation.API.Configurations;
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
            services.AddTestCreationPersistence(Configuration.GetConnectionString("TestCreationDbContext"));
            services.AddTestCreationApplicationServices();
            services.AddTestCreationInfrastructureServices();

            services.AddUserManagementPersistence(Configuration.GetConnectionString("UserManagementDbContext"));
            services.AddUserManagementApplicationServices();
            services.AddUserManagementInfrastructureServices();

            services.AddJWTAuthentication(Configuration); 
            services.AddControllers();
            services.AddCORS();
            services.AddOpenAPI();
            services.AddResponseCaching();            
            services.AddHealthChecks();

            services.AddHostedService<PostManService>();
            services.AddSingleton<IEventBus, InMemoryEventBus>();

            services.AddHttpContextAccessor();
            services.AddScoped<ICorrelationIdProvider, CorrelationIdProvider>();
        }   
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.SubscribeTestCreationEventHandlers();
           
            if (env.IsDevelopment())
            {
                app.UseOpenAPI();
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
    }
}