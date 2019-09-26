using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using TestMe.Presentation.API.Configurations;
using TestMe.TestCreation.App;
using TestMe.TestCreation.Persistence;
using TestMe.UserManagement.App;
using TestMe.UserManagement.Persistence;

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

            services.AddUserManagementPersistence(Configuration.GetConnectionString("UserManagementDbContext"));
            services.AddUserManagementApplicationServices();

            services.AddJWTAuthentication(Configuration); 
            services.AddControllers();            

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins("https://localhost:44362", "https://testme.mw-neves.pl")                   
                                      .AllowAnyMethod()
                                      .AllowAnyHeader()
                                      .AllowCredentials()
                .Build());
            });

            services.AddResponseCaching();            
            services.AddHealthChecks();  
        }  
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
            app.UseCors("CorsPolicy"); 
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