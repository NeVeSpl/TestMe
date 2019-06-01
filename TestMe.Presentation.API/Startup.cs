using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
        private ILogger<Startup> Logger { get; }


        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            Logger = logger;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            Logger.LogDebug("ConfigureServices begin");

            services.AddTestCreationPersistence(Configuration.GetConnectionString("TestCreationDbContext"));
            services.AddTestCreationApplicationServices();

            services.AddUserManagementPersistence(Configuration.GetConnectionString("UserManagementDbContext"));
            services.AddUserManagementApplicationServices();

            services.AddJWTAuthentication(Configuration);
           
            var mvcBuilder = services.AddMvcCore().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            mvcBuilder.AddApiExplorer();
            mvcBuilder.AddAuthorization();
            mvcBuilder.AddCors();
            mvcBuilder.AddJsonFormatters();
            mvcBuilder.AddDataAnnotations();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.WithOrigins("https://localhost:44362", "https://testme.mw-neves.pl")                   
                                      .AllowAnyMethod()
                                      .AllowAnyHeader()
                                      .AllowCredentials()
                .Build());
            });
            
            services.AddSwaggerDocument();

            Logger.LogDebug("ConfigureServices end");
        }  
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            Logger.LogDebug("Configure begin");            

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
            app.UseNSwag();            
            app.UseCors("CorsPolicy");           
            app.UseAuthentication();
            app.UseMvc();

            Logger.LogDebug("Configure end");
        }
    }
}