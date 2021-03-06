﻿using System.Globalization;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TimeRegistration.BusinessLogic.Interfaces;
using TimeRegistration.BusinessLogic.Repositories;
using TimeRegistration.BusinessLogic.Services;

namespace TimeRegistration.Web
{
    public class Startup
    {
        private readonly IConfigurationRoot configuration;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            this.configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton<IRepository, InMemoryRepository>();
            services.AddTransient<IReportGeneratorService, ReportGeneratorService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(this.configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            // Force to only support English culture
            var englishCulture =  CultureInfo.InvariantCulture;
            app.UseRequestLocalization(new RequestLocalizationOptions() { SupportedCultures = new[] { englishCulture } }, new RequestCulture(CultureInfo.InvariantCulture));

            app.UseIISPlatformHandler();
            app.UseStaticFiles();
            app.UseMvc();
        }

        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
