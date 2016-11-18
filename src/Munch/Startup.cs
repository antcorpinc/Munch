using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace Munch
{
    public class Startup
    {

        public Startup(IHostingEnvironment hostingEnvironment)
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(hostingEnvironment.ContentRootPath)
                 .AddJsonFile("config.json")
                 //All environment variables in the process's context flow in as configuration values.
                 .AddEnvironmentVariables();

            Configuration = builder.Build();

        }
        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // Add framework services.
            services.AddMvc(o => o.Conventions.Add(new FeatureConvention()))
                .AddRazorOptions(options =>
                {
                    // {0} - Action Name
                    // {1} - Controller Name
                    // {2} - Area Name
                    // {3} - Feature Name
                    options.AreaViewLocationFormats.Clear();
                    options.AreaViewLocationFormats.Add("/Areas/{2}/Features/{3}/{1}/{0}.cshtml");
                    options.AreaViewLocationFormats.Add("/Areas/{2}/Features/{3}/{0}.cshtml");
                    options.AreaViewLocationFormats.Add("/Areas/{2}/Features/Shared/{0}.cshtml");
                    options.AreaViewLocationFormats.Add("/Areas/Shared/{0}.cshtml");

                    // replace normal view location entirely
                    options.ViewLocationFormats.Clear();
                    options.ViewLocationFormats.Add("/Features/{3}/{1}/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Features/{3}/{0}.cshtml");
                    options.ViewLocationFormats.Add("/Features/Shared/{0}.cshtml");

                    // add support for features side-by-side with /Views
                    // (do NOT clear ViewLocationFormats)
                    //options.ViewLocationFormats.Insert(0, "/Features/Shared/{0}.cshtml");
                    //options.ViewLocationFormats.Insert(0, "/Features/{3}/{0}.cshtml");
                    //options.ViewLocationFormats.Insert(0, "/Features/{3}/{1}/{0}.cshtml");
                    //
                    // (do NOT clear AreaViewLocationFormats)
                    //options.AreaViewLocationFormats.Insert(0, "/Areas/{2}/Features/Shared/{0}.cshtml");
                    //options.AreaViewLocationFormats.Insert(0, "/Areas/{2}/Features/{3}/{0}.cshtml");
                    //options.AreaViewLocationFormats.Insert(0, "/Areas/{2}/Features/{3}/{1}/{0}.cshtml");


                    options.ViewLocationExpanders.Add(new FeatureViewLocationExpander());
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
