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
using HighFive.Server.Api.Models;
using HighFive.Server.ViewModels;
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
            services.AddDbContext<HighFiveContext>(options => options.UseSqlServer(connection));

            //.AddEntityFrameworkStores<HighFiveContext>();
            services.AddScoped<IHighFiveRepository, HighFiveRepository>();
            services.AddTransient<HighFiveContextSeedData>();

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
                //config.CreateMap<UserViewModel, Organization>().ReverseMap();
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

            app.UseMvc();

            seeder.EnsureSeedData().Wait();
        }
    }
}
