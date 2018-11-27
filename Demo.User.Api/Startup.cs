using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Demo.User.Api
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
            var redis = ConnectionMultiplexer.Connect("192.168.1.121,connectTimeout=5000,password=antoine123!@#");
            // 确保每个应用程序的此属性值都一样
            services.AddDataProtection(options => options.ApplicationDiscriminator = "oneaspnet")
                .PersistKeysToRedis(redis, "onekey");

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.Cookie.Domain = ".domain.localhost";
                    options.Cookie.Name = "sso";
                    options.Cookie.Path = "/";
                    options.LoginPath = "/User/SignIn";
                    options.AccessDeniedPath = "/User/Deny";
                    options.Events = new CookieAuthenticationEvents
                    {
                        OnValidatePrincipal = LastChangedValidator.ValidateAsync
                    };
                });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //// 不允许匿名访问
            //services.AddMvc(options =>
            //{
            //    var policy = new AuthorizationPolicyBuilder()
            //                     .RequireAuthenticatedUser()
            //                     .Build();
            //    options.Filters.Add(new AuthorizeFilter(policy));
            //}).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();
            app.UseMvc();
        }
    }

    public static class LastChangedValidator
    {
        public static async Task ValidateAsync(CookieValidatePrincipalContext context)
        {
            if (false)
            {
                context.RejectPrincipal();
                await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }
    }
}
