using System;
using System.Data.SqlClient;
using System.IO;
using System.Net.Http;
using Dapper;
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
      
      ConnectionString = configuration.GetConnectionString("MasterConnection");
      using var connection = new SqlConnection(ConnectionString);
      connection.Execute(DefaultSqlScripts.CreateDb());
      connection.Execute(DefaultSqlScripts.CreateTables());
      
      ConnectionString = configuration.GetConnectionString("DefaultConnection");
    }
  }
}