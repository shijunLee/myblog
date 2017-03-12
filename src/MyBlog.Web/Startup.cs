using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System.Text;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using MyBlog.Common.Cache;
using MyBlog.Common.Tool;
using MyBlog.Common.DapperTool;
using MyBlog.Model.DataModel;
using MyBlog.DAL.Admin.Login;
using MyBlog.DAL;
using MyBlog.Model.Command;
using MyBlog.Model.ViewModel;
using MyBlog.DAL.View.PostView;
using MyBlog.DAL.Admin;
using MyBlog.Model.DataBind;
using MyBlog.DAL.View;
using MyBlog.DAL.View.Tag;
using MyBlog.Model.config;
using MyBlog.DAL.Admin.IndexInfo;
using MyBlog.Model.ViewModel.Admin;
using MyBlog.DAL.Admin.PostManager;
using MyBlog.DAL.Admin.TagManager;
using MyBlog.DAL.Admin.UserManager;

namespace MyBlog.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ICache, RuntimeMemoryCache>();
            services.AddScoped<IVerificationCodeTool, VerificationCodeTool>();
            services.AddScoped<INetWorkTool, NetWorkTool>();
            services.AddScoped<IDapperConnection, DapperConnection>();
            services.AddScoped<ICommandInvoker<AdminUserInitCommand, CommonResult>, AdminUserInitCommandInvoker>();
            services.AddScoped<ICommandInvoker<LoginCommand, CommonResult>, LoginCommandInvoker>();
            services.AddScoped<IViewDisplay<PostListDataBind, PageInfo<PostListViewModel>>, PostListViewInvorker>();
            services.AddScoped<IViewDisplay<TagCloudDataBind, List<TagCloudViewModel>>, BlogTagCloudViewInvorker>();
            services.AddScoped<IViewDisplay<TagPostsDataBind, List<TagPostsViewModel>>, TagPostsViewInvorker>();
            services.AddScoped<IViewDisplay<PostDetailDataBind, PostDetialViewModel>, PostDetialViewInvorker>();
            services.AddScoped<ICommandInvoker<IndexInfoCommand, CommonResult<IndexInfoViewModel>>, IndexInfoCommandInvorker>();
            services.AddScoped<ICommandInvoker<PostIndexCommand, List<PostManagerListViewModel>>, PostManagerIndexCommandInvorker>();
            services.AddScoped<ICommandInvoker<AddPostCommand, CommonResult>, AddPostCommandInvorker>();
            services.AddScoped<ICommandInvoker<GetPostInfoCommand, PostInfoVIewModel>,GetPostInfoCommandInvorker>();
            services.AddScoped<ICommandInvoker<UpdatePostCommand, CommonResult>, UpdatePostCommandInvorker>();
            services.AddScoped<ICommandInvoker<TagManagerCommand, List<TagManagerListViewModel>>, TagListCommandInvorker>();
            services.AddScoped<ICommandInvoker<ModifyUserCommand, CommonResult>, ModifyUserInfoCommandInvorker>();
            services.AddScoped<ICommandInvoker<GetUserCommand, UserInfoViewModel>, GetUserInfoCommandInvorker>();
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));
            services.Configure<AdminUserConfig>(Configuration.GetSection("AdminUserConfig"));
            services.AddOptions();

            services.AddSession(options =>
            {
                options.CookieName = ".AdventureWorks.Session";
                options.CookieHttpOnly = true;
                options.IdleTimeout = TimeSpan.FromSeconds(5000);
            });
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddMvc();

            services.AddAntiforgery();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            loggerFactory.AddDebug();
            loggerFactory.AddNLog();
            app.UseApplicationInsightsRequestTelemetry();


            var dataProtection = DataProtectionProvider.Create(env.ContentRootPath + "\\keys");
            // var dataProtection = new DataProtectionProvider(new DirectoryInfo(@"C:\keys"));// no use UNC share
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = "MyCookieMiddlewareInstance",
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                CookieHttpOnly = true,
                ExpireTimeSpan = TimeSpan.FromMinutes(43200),
                LoginPath = new PathString("/Admin/login"),
                CookieName = ".MvcCoreLearnCookie",
                CookiePath = "/",
                DataProtectionProvider = dataProtection
            });
            //app.Use(next => content => {
            //    var tokens = antiforgery.GetAndStoreTokens(context);
            //    context.Response.Cookies.Append(
            //        "XSRF-TOKEN",
            //        tokens.RequestToken,
            //        new CookieOptions() { HttpOnly = false }
            //    );

            //   // return next(context);
            //});


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseApplicationInsightsExceptionTelemetry();

            app.UseStaticFiles();
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            TypeMapper.Initialize(typeof(UserInfo));
            //TypeMapper.Initialize(typeof(Post));
            TypeMapper.Initialize(typeof(BlogTag));
            TypeMapper.Initialize(typeof(SpamHash));
            TypeMapper.Initialize(typeof(SpamShield));
            TypeMapper.Initialize(typeof(PostNev));
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }
    }
}
