using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicJournal.Application.Academic.AcademicSubjects;
using ElectronicJournal.Application.Academic.AcademicSubjectScores;
using ElectronicJournal.Application.Academic.HomeWorks;
using ElectronicJournal.Application.Academic.StudyGroups;
using ElectronicJournal.Application.Authorization.Users;
using ElectronicJournal.Application.Navigation;
using ElectronicJournal.Application.PrepareInitialize;
using ElectronicJournal.Core.Authorization.Roles;
using ElectronicJournal.Core.Authorization.Users;
using ElectronicJournal.EntityFrameworkCore.Data;
using ElectronicJournal.EntityFrameworkCore.Data.Repositories;
using ElectronicJournal.Web.Areas.Startup;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ElectronicJournal.Web.Startup
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
            services.AddDbContext<ElectronicJournalDbContext>(
                options => options.UseMySQL(Configuration.GetConnectionString("Default")));
           
            services.AddIdentity<User, Role>(options => ConfigureIdentityOptions(options))
                .AddEntityFrameworkStores<ElectronicJournalDbContext>();
            services.AddAuthorization();
            services.AddScoped(typeof(IRepository<,>), typeof(ElectronicJournalRepositoryBase<,>));
            ConfigureCookies(services);
            ConfigurePrepareInitialize(services);
            ConfigureNaviations(services);
            ConfigureApplicationServices(services);

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/Error", "?code={0}");
                app.UseExceptionHandler("/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("dafaultWithArea", "{area}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
        private void ConfigureApplicationServices(IServiceCollection services)
        {
            //Users
            services.AddScoped<IUserAppService, UserAppService>();
            services.AddScoped<ITeacherAppService, TeacherAppService>();
            services.AddScoped<IStudentAppService, StudentAppService>();

            //Academic
            services.AddScoped<IAcademicSubjectAppService, AcademicSubjectAppService>();
            services.AddScoped<IStudyGroupAppService, StudyGroupAppService>();
            services.AddScoped<IAcademicSubjectScoreAppService, AcademicSubjectScoreAppService>();
            services.AddScoped<IHomeWorkAppService, HomeWorkAppService>();
        }
        private void ConfigureIdentityOptions(IdentityOptions options)
        {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.User.RequireUniqueEmail = true;
        }
        private void ConfigureCookies(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromHours(1);
                options.SlidingExpiration = true;
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.Cookie = new CookieBuilder
                {
                    IsEssential = true,
                    Name = "LoginInfo",
                    HttpOnly = true
                };
            });
        }
        private void ConfigureNaviations(IServiceCollection services)
        {
            services.AddSingleton<INavigationManager, NavigationManager>();
            services.AddTransient<NavigationProvider, ElectronicJournalNavigationProvider>();
            services.AddTransient<IUserNavigationManager, UserNavigationManager>();
            services.AddHostedService<NavigationManagerHostedService>();
        }
        private void ConfigurePrepareInitialize(IServiceCollection services)
        {
            services.AddScoped<IPrepareInitializeAppService, PrepareInitializeAppService>();
            services.AddHostedService<PrepareInitializeHostedService>();
        }
    }
}
