using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using FFValidationApp_glp.Controller;
using Process = FFValidationApp_glp.Controller.Process;

namespace FFValidationApp_glp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();
            var process = ActivatorUtilities.CreateInstance<Process>(host.Services);
            process.Run();
            host.Run();

        }
        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
         .ConfigureAppConfiguration((hostingContext, configuration) =>
         {
             configuration.Sources.Clear();
             configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
             configuration.AddEnvironmentVariables();
         })
         .ConfigureServices((hostContext, services) =>
         {
             services.AddLogging(builder =>
             {
                 builder.AddConsole();
             });
             services.AddTransient<Process>();
         });

    }
}