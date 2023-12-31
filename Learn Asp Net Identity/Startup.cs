using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Learn_Asp_Net_Identity.AuthorizationRequirements;
using Microsoft.AspNetCore.Authorization;

namespace Learn_Asp_Net_Identity
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
            services.AddRazorPages();
            services.AddAuthentication().AddCookie("MyCookie", options =>
            {
                options.Cookie.Name = "MyCookie";
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireClaim("Admin"));
                options.AddPolicy("MustBelongToHRDepartment", policy => policy.RequireClaim("Department", "HR"));

                options.AddPolicy("HrManagerOnly",
                    policy => policy
                        .RequireClaim("Department", "HR")
                        .RequireClaim("Manager")
                        .Requirements.Add(new HRManagerAuthRequirements(3))
                );
            });
            services.AddSingleton<IAuthorizationHandler, HRManagerAuthRequirements.HRManagerProbationHandler>();

            services.AddHttpClient(
                "OurWebApiClient", client => { client.BaseAddress = new Uri("http://localhost:5254/"); });

            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.IdleTimeout = TimeSpan.FromDays(1);
                options.Cookie.IsEssential = true;
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });
        }
    }
}