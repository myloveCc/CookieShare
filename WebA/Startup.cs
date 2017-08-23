using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieInstance;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebA
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthInfo.CookieInstance)
                 .AddCookie(CookieAuthInfo.CookieInstance, options =>
                 {
                     options.LoginPath = new PathString("/Account/Login");
                     options.AccessDeniedPath = new PathString("/Account/Denied");
                     options.LogoutPath = new PathString("/Account/Logout");
                     options.Cookie.Domain = "cookie.com";
                     options.Cookie.SameSite = SameSiteMode.Lax;
                 });

            services.AddDataProtection()
                .SetApplicationName("cookieshare")
                //windows、Linux、macOS 下可以使用此种方式 保存到文件系统
                .PersistKeysToFileSystem(new System.IO.DirectoryInfo("C:\\share_keys"));

            services.AddDataProtection()
                .SetApplicationName("cookieshare")
                .AddKeyManagementOptions(options =>
                {
                    options.XmlRepository = new XmlRepository();
                });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();

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
