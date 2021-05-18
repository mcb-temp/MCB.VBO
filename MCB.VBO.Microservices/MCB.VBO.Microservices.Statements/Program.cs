using System;
using System.IO;
using App.Metrics;
using App.Metrics.AspNetCore;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MCB.VBO.Microservices.Statements
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                //#if REPORTING
                .ConfigureMetricsWithDefaults(builder =>
                {
                    //builder.Report.ToConsole(TimeSpan.FromSeconds(2));
                    builder.Report.ToTextFile(Path.Combine(AppContext.BaseDirectory, $"metrics_{DateTime.Now.ToString("yyyyMMdd")}.json"), TimeSpan.FromSeconds(20));
                })
                //#endif
#if HOSTING_OPTIONS
                .ConfigureAppMetricsHostingConfiguration(options =>
                {
                    // options.AllEndpointsPort = 3333;
                    options.EnvironmentInfoEndpoint = "/my-env";
                    options.EnvironmentInfoEndpointPort = 1111;
                    options.MetricsEndpoint = "/my-metrics";
                    options.MetricsEndpointPort = 2222;
                    options.MetricsTextEndpoint = "/my-metrics-text";
                    options.MetricsTextEndpointPort = 3333;
                })
#endif
                .UseMetricsWebTracking()
                .UseMetricsEndpoints()
                .UseMetrics()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
