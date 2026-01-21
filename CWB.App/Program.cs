using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NLog;
using System;
using System.Diagnostics.CodeAnalysis;

namespace CWB.App
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
        {
            StartupFileLogger.Log("Program.Main started");
            try
            {
                StartupFileLogger.Log("Building host");
                CreateHostBuilder(args).Build().Run();
                StartupFileLogger.Log("Host exited normally");
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Error(ex, "CWB APP startup failed");
                StartupFileLogger.Log(
                    $"Program.Main FAILED: {ex}\n"
                );
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    StartupFileLogger.Log("Configuring WebHost");
                    webBuilder.UseStartup<Startup>();
                });
    }
}
