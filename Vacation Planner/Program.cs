using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace VacationPlanner
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var logger = NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();

      try
      {
        logger.Debug("Init main.");
        CreateHostBuilder(args).Build().Run();
      }
      catch (Exception e)
      {
        logger.Error(e, e.Message);
        throw;
      }
      finally
      {
        NLog.LogManager.Shutdown();
      }
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
      return Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); }).ConfigureLogging(logging =>
        {
          logging.ClearProviders();
          logging.SetMinimumLevel(LogLevel.Trace);
        })
        .UseNLog();
      ;
    }
  }
}