using E_ComerCore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace E_ComerCore
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
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "User_Schema";
            })
            .AddCookie("User_Schema", options =>
            {
                options.LoginPath = "/account/login";
                options.LogoutPath = "/account/signout";
                options.AccessDeniedPath = "/account/accessdenied";
            })
            .AddCookie("Admin_Schema", options =>
            {
                options.LoginPath = "/admin/login/index";
                options.LogoutPath = "/admin/login/signout";
                options.AccessDeniedPath = "/admin/login/accessdenied";
            });
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            //{
            //    options.LoginPath = "/admin/login/index";
            //    options.LogoutPath = "/admin/login/signOut";
            //    options.AccessDeniedPath = "/admin/account/accessdenied";

            //});

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;

            });
            var connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DatabaseContext>(options => options.UseLazyLoadingProxies().UseSqlServer(connection));

     

            services.AddSession();
            services.AddMvc();
            services.AddMvc().AddSessionStateTempDataProvider();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseForwardedHeaders();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                //app.UseForwardedHeaders();
                //app.UseHsts();
            }

            app.Use(async (context, next) =>
            {
                var principal = new ClaimsPrincipal();
                var result1 = await context.AuthenticateAsync("User_Schema");
                if (result1?.Principal != null)
                {
                    principal.AddIdentities(result1.Principal.Identities);
                }
                var result2 = await context.AuthenticateAsync("Admin_Schema");
                if (result2?.Principal != null)
                {
                    principal.AddIdentities(result2.Principal.Identities);
                }
                context.User = principal;
                await next();
            });

            app.UseAuthentication();

            app.UseSession();

            app.UseStaticFiles();

            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                //routes.MapRoute(
                //     name: "Area",
                //     template: "{area:exists}/{controller=Login}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });


        }
    }
}
