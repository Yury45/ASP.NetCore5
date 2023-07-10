using DotNetCore5.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore5
{
    public class Startup
    {
        public static IWebHostEnvironment _env;

        public Startup(IWebHostEnvironment env)
        {
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseMiddleware<LoggingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.Map("/", async context => {
                    Console.WriteLine();
                    var viewPath = Path.Combine(Directory.GetCurrentDirectory(), "Views", "index.html");
                    var html = await File.ReadAllTextAsync(viewPath);

                    Console.WriteLine(html);

                    await context.Response.WriteAsync(html.ToString());
                });
            });

            app.Map("/about", About);
            app.Map("/config", Config);

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync($"Page not found!");
            });
        }

        #region EndpointMaps

        private static void About(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync($"{_env.ApplicationName} - ASP.Net Core tutorial project");
            });
        }
        private static void Config(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync($"App name: {_env.ApplicationName}. App running configuration: {_env.EnvironmentName}");
            });
        }
        #endregion
    }
}
