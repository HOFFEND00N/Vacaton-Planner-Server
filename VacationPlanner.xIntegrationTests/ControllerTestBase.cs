using System;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace VacationPlanner.xIntegrationTests
{
  public class ControllerTestBase
  {
    protected readonly HttpClient HttpClient;
    protected readonly string ConnectionString;
    
    public ControllerTestBase()
    {
      HttpClient = new WebApplicationFactory<Startup>().WithWebHostBuilder(_ => { })
        .CreateClient();

      var basePath = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName;
      var configuration = new ConfigurationBuilder().SetBasePath(basePath).AddJsonFile("test_appsettings.json").Build();
      ConnectionString = configuration.GetConnectionString("DBConnectionString");
    }
  }
}