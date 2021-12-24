using Autofac;
using Blog.App.WebApp.Dependency;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Blog.App.WebApp
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
            services.AddControllersWithViews();
            services.AddSession();

            services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.All }));
            services.AddHttpContextAccessor();
            services.AddMemoryCache();
            services.AddAuthentication("CookieAuthentication")
                .AddCookie("CookieAuthentication", config =>
                {
                    config.Cookie.Name = "UserLoginCookie";
                    config.ExpireTimeSpan = TimeSpan.FromDays(1);
                    config.LoginPath = "/dang-nhap.html";
                   // config.LogoutPath = "/dang-xuat.html";
                   // config.AccessDeniedPath = "/not-found.html";
                });
            services.ConfigureApplicationCookie(option =>
            {
                option.Cookie.HttpOnly = true;
                option.Cookie.Expiration = TimeSpan.FromMilliseconds(1000);
                option.ExpireTimeSpan = TimeSpan.FromMilliseconds(1000);
            });
        }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            #region WAY-1 (Autofac Module)

            // Add modules registrations.
            builder.RegisterModule(new EFModule());
            builder.RegisterModule(new RepositoryModule());
            builder.RegisterModule(new ServiceModule());

            #endregion

            #region WAY-2 (Direct Registration)

            // Add services registrations.

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSession();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
