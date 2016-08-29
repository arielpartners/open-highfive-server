#region references

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using System.IO;
using HighFive.Server.Services.Models;
using HighFive.Server.Web.ViewModels;
using Microsoft.EntityFrameworkCore;

#endregion

namespace HighFive.Server
{
    public class Startup
    {
        private IHostingEnvironment _env;
        private IConfigurationRoot _config;

        public Startup(IHostingEnvironment env)
        {
            _env = env;
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(env.ContentRootPath, "Config"))
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            _config = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_config);

            var connection = _config["ConnectionStrings:HighFiveContextConnection"];
            services.AddDbContext<HighFiveContext>(options => options.UseSqlServer(connection,
                b => b.MigrationsAssembly("HighFive.Server")));
            //
            // Set migrations assembly to top level because it defaults to the same assmebly that the DbContext is in,
            // but dotnet EF core does not yet support migrations from a class library
            //

            //.AddEntityFrameworkStores<HighFiveContext>();
            services.AddScoped<IHighFiveRepository, HighFiveRepository>();
            services.AddTransient<HighFiveContextSeedData>();

            services.AddIdentity<HighFiveUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit = false;
                config.Password.RequireLowercase = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents()
                {
                    OnRedirectToLogin = async ctx =>
                    {
                        if (ctx.Request.Path.StartsWithSegments("/api") &&
                        ctx.Response.StatusCode == 200)
                        {
                            ctx.Response.StatusCode = 401;
                        }
                        else
                        {
                            ctx.Response.Redirect(ctx.RedirectUri);
                        }
                        await Task.Yield();
                    }
                };
            }).AddEntityFrameworkStores<HighFiveContext>();

            services.AddLogging();

            // Add framework services.
            services.AddMvc()
                .AddJsonOptions(config =>
                {
                    // make sure serialized JSON is camel-cased rather than pascal-cased.
                    config.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            HighFiveContextSeedData seeder,
            ILoggerFactory loggerFactory)
        {
            // startup in Configure

            Mapper.Initialize(config =>
            {
                config.CreateMap<UserViewModel, HighFiveUser>().ReverseMap();
                config.CreateMap<HighFiveUser, UserViewModel>()
                    .ForMember(g => g.OrganizationName, opt => opt.MapFrom(u => u.Organization.Name))
                    .ForMember(g => g.Url, opt => opt.MapFrom(u => u.Organization.Url));
            });

            if (env.IsEnvironment("Development"))
            {
                //app.UseDeveloperExceptionPage();
                loggerFactory.AddDebug(LogLevel.Information);
                loggerFactory.AddConsole(_config.GetSection("Logging"));
                loggerFactory.AddDebug();
            }
            else
            {
                loggerFactory.AddDebug(LogLevel.Error);
            }

            app.UseIdentity();

            app.UseMvc();

            seeder.EnsureSeedData().Wait();
        }
    }
}
