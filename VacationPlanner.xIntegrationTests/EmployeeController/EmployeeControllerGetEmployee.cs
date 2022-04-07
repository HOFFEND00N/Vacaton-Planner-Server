using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Http;
using Dapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using VacationPlanner.Models;
using Xunit;

namespace VacationPlanner.xIntegrationTests.EmployeeController
{
  [Collection("CollectionForSequentialTestRunning")]
  public class EmployeeControllerGetEmployee : IDisposable
  {
    private readonly HttpClient HttpClient;
    private List<Employee> _employees;
    private readonly string _connectionString;

    public EmployeeControllerGetEmployee()
    {
      HttpClient = new WebApplicationFactory<Startup>().WithWebHostBuilder(_ => { })
        .CreateClient();

      var basePath = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName;
      var configuration = new ConfigurationBuilder().SetBasePath(basePath).AddJsonFile("test_appsettings.json").Build();
      _connectionString = configuration.GetConnectionString("DBConnectionString");

      using var connection = new SqlConnection(_connectionString);
      connection.Execute(DefaultSqlScripts.CreateEmployeeTestData());
      _employees = (List<Employee>) connection.Query<Employee>(DefaultSqlScripts.SelectEmployeeTestData());
    }

    [Fact]
    public async void ShouldGetEmployee()
    {
      var expectedEmployee = _employees[1];
      expectedEmployee.Vacations = new List<Vacation>();
      var response = await HttpClient.GetAsync($"Employee/{expectedEmployee.Id}");

      response.StatusCode.Should().Be(HttpStatusCode.OK);
      var actualEmployee = JsonConvert.DeserializeObject<Employee>(await response.Content.ReadAsStringAsync());
      actualEmployee.Should().BeEquivalentTo(expectedEmployee);
    }

    [Fact]
    public async void ShouldReturnNotFoundResultWhenRequestForNonExistingEmployee()
    {
      var response = await HttpClient.GetAsync("Employee/100");

      response.StatusCode
        .Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async void ShouldReturnNotFoundResultWhenRequestIsIncorrect()
    {
      var response = await HttpClient.GetAsync("Employee/asd");

      response.StatusCode
        .Should().Be(HttpStatusCode.NotFound);
    }

    public void Dispose()
    {
      using var connection = new SqlConnection(_connectionString);
      connection.Execute(DefaultSqlScripts.DeleteEmployeeTestData());
    }
  }
}