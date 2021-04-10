using MicroserviceSample.HostedServices;
using MicroserviceSample.Services;
using MicroserviceSample.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;

namespace MicroserviceSample
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                 .ConfigureAppConfiguration((hostingContext, config) => AppConfiguration(hostingContext, config))
                 .UseSerilog()
                 .ConfigureServices((hostContext, services) => ConfigureServices(hostContext, services));
        }

        private static void AppConfiguration(HostBuilderContext hostingContext, IConfigurationBuilder config)
        {
            config.Sources.Clear();
            var env = hostingContext.HostingEnvironment;

            config.AddJsonFile("appsettings.json")
                  .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                  .AddJsonFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", "config.json"))
                  .AddJsonFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", $"config.{env.EnvironmentName}.json"), optional: true)
                  .AddEnvironmentVariables();

            Log.Logger = new LoggerConfiguration()
               .ReadFrom.Configuration(config.Build())
               .CreateLogger();
        }

        private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.AddHostedService<TimedHostedService>();
            services.AddHostedService<QueuedHostedService>();
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();

            services.AddTransient<ICloudflareService, CloudflareService>();
            services.AddTransient<IPersistenceService, PersistenceService>();

            services.Configure<ProcessSettings>(hostContext.Configuration.GetSection("AppSettings"));
            services.Configure<CloudflareSettings>(hostContext.Configuration.GetSection("CloudflareSettings"));

            services.AddSingleton(hostContext.Configuration.GetSection("DDNSSettings").Get<IEnumerable<DDNSSettings>>());
        }
    }
}
