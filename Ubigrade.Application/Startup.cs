using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Ubigrade.Application.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using Google.Apis.Admin.Directory.directory_v1;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace Ubigrade.Application
{
    public class Startup
    {
        private static readonly string[] scopes = new[] {

                "https://www.googleapis.com/auth/drive",
                "https://www.googleapis.com/auth/classroom.coursework.students.readonly",
                "https://www.googleapis.com/auth/classroom.coursework.me.readonly",
                "https://www.googleapis.com/auth/classroom.course-work.readonly",
                "https://www.googleapis.com/auth/classroom.coursework.students",
                "https://www.googleapis.com/auth/classroom.coursework.me",
                "https://www.googleapis.com/auth/classroom.courses",
                "https://www.googleapis.com/auth/classroom.rosters.readonly",
                DirectoryService.Scope.AdminDirectoryUser,
                DirectoryService.Scope.AdminDirectoryOrgunit,
                DirectoryService.Scope.AdminDirectoryOrgunitReadonly,
                DirectoryService.Scope.AdminDirectoryUserReadonly,
                "https://www.googleapis.com/auth/user.organization.read",
                "https://www.googleapis.com/auth/userinfo.profile"
            };

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<UbigradeDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<UbigradeDbContext>().AddSignInManager<GoogleAwareSignInManager>();
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddAuthentication()
               .AddGoogle(options =>
               {
                   options.ClientId = "519381508568-vtbjnd18kib10044i4i4e2ot4l6nu8ca.apps.googleusercontent.com";
                   options.ClientSecret = "l_FocfSrofdeoh3bFuS6r4Qt";
                   foreach (var s in scopes)
                   {
                       options.Scope.Add(s);
                   }
                   options.AccessType = "offline";
                   options.SaveTokens = true;
                   //options.Events = new OAuthEvents
                   //{
                   //    OnCreatingTicket = context =>
                   //    {
                   //        var language = context.User.tryget
                   //        context.Identity.AddClaim(new Claim("urn:language", language));
                   //        return Task.FromResult(0);
                   //    }
                   //};
               });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    //teste grad ob developement fehler ned kommt wenn ich das auskommentier
            //    app.UseDeveloperExceptionPage();
            //    app.UseDatabaseErrorPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}
            if (env.IsProduction())
            {
                //app.UseHostFiltering();
            } 
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
