using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TestMe.Presentation.React
{
    public class Startup
    {
        public IConfiguration Configuration { get; }


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        

        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.Use(async (context, nextMiddleware) =>
            {
                context.Response.OnStarting(() =>
                {
                    if (context.Response.ContentType != null)
                    {
                        if (context.Response.ContentType.StartsWith("text/html"))
                        {
                            if (env.IsDevelopment())
                            {
                                // In development React add-on to browser requires script-src set to unsafe-inline
                                //               React-Hot-Loader requires script-src set to unsafe-eval
                                //               Webpack requires style-src set to unsafe-inline
                                context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; connect-src localhost:* wss://localhost:* ws://localhost:*; font-src data:; img-src * data:; script-src 'self' 'unsafe-inline' 'unsafe-eval' *.googletagmanager.com *.google-analytics.com;  style-src 'self' 'unsafe-inline';");
                            }
                            else
                            {
                                context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; connect-src localhost:* testme-api.mw-neves.pl:* wss://localhost:* ws://localhost:*; font-src data:; img-src * data:; script-src 'self' *.googletagmanager.com *.google-analytics.com;  style-src 'self';");
                            }
                            context.Response.Headers["X-Frame-Options"] = "deny";
                            context.Response.Headers["X-Content-Type-Options"] = "nosniff";
                            context.Response.Headers["Referrer-Policy"] = "same-origin";
                            context.Response.Headers["X-Xss-Protection"] = "1; mode=block";
                            context.Response.Headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains";
                        }
                        if (!env.IsDevelopment())
                        {
                            if (context.Response.ContentType.StartsWith("application/javascript") || context.Response.ContentType.StartsWith("text/css"))
                            {
                                context.Response.Headers["Cache-Control"] = "public, max-age=31536000";
                            }
                        }
                    }

                    return Task.CompletedTask;
                });
                await nextMiddleware();
            });


            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
