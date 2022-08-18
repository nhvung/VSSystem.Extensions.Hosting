using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace VSSystem.Extensions.Hosting
{
    public partial class AStartup
    {
        protected string _defaultPattern;
        protected List<string> _controller_names;
        public AStartup(string defaultPattern = "{controller}/{action}/{id?}")
        {
            _defaultPattern = defaultPattern;
            _controller_names = new List<string>();
        }
        protected virtual void _ConfigureMiddleware(IApplicationBuilder app, IWebHostEnvironment env)
        {

        }
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(opts =>
                {
                    opts.EnableEndpointRouting = false;
                }
            );
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();
            _ConfigureMiddleware(app, env);
            app.UseRouting();
            app.UseEndpoints(opts =>
            {
                opts.MapControllerRoute(
                    name: "default",
                    pattern: _defaultPattern
                );
            });
        }
    }
}
