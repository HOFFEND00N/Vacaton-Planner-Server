using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using Dapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using VacationPlanner.DataAccess.Models;
using VacationPlanner.Models;
using Xunit;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace VacationPlanner.xIntegrationTests.VacationController
{
  [Collection("CollectionForSequentialTestRunning")]
  public class VacationControllerEditVacation: IDisposable
  {
    private readonly HttpClient HttpClient;
    private readonly string _connectionString;
    private readonly List<DataVacation> _vacations;

    public VacationControllerEditVacation()
    {
      HttpClient = new WebApplicationFactory<Startup>().WithWebHostBuilder(_ => { })
        .CreateClient();

      var basePath = Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName;
      var configuration = new ConfigurationBuilder().SetBasePath(basePath).AddJsonFile("test_appsettings.json").Build();
      _connectionString = configuration.GetConnectionString("DBConnectionString");

      using var connection = new SqlConnection(_connectionString);
      connection.Execute(DefaultSqlScripts.CreateEmployeeTestData());
      connection.Execute(DefaultSqlScripts.CreateVacationTestData());
      _vacations = (List<DataVacation>) connection.Query<DataVacation>(DefaultSqlScripts.SelectVacationTestData());
    }

    [Fact]
    public async void ShouldUpdateVacation()
    {
      var expectedVacation = _vacations[0];
      expectedVacation.Start = DateTime.Now.AddDays(10);
      expectedVacation.End = DateTime.Now.AddDays(20);
      var content = new StringContent(
        JsonSerializer.Serialize(expectedVacation),
        Encoding.UTF8,
        "application/json"
      );

      var response = await HttpClient.PutAsync($"Employee/{expectedVacation.EmployeeId}/vacation/{expectedVacation.Id}",
        content);

      response.StatusCode.Should().Be(HttpStatusCode.OK);
      var actualVacation = JsonConvert.DeserializeObject<Vacation>(response.Content.ReadAsStringAsync().Result);
      actualVacation.End.Should().Be(expectedVacation.End.Date);
      actualVacation.Start.Should().Be(expectedVacation.Start.Date);
      actualVacation.VacationState.Should().Be(expectedVacation.State);
    }

    [Fact]
    public async void ShouldReturnNotFoundResultWhenRequestForNonExistentEmployee()
    {
      var expectedVacation = new Vacation
      {
        Start = DateTime.Now.AddDays(10),
        End = DateTime.Now.AddDays(20)
      };
      var content = new StringContent(
        JsonSerializer.Serialize(expectedVacation),
        Encoding.UTF8,
        "application/json"
      );

      var response = await HttpClient.PutAsync("Employee/100/vacation/1002", content);

      response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async void ShouldReturnNotFoundResultWhenRequestForNonExistentVacation()
    {
      var expectedVacation = new Vacation
      {
        Start = DateTime.Now.AddDays(10),
        End = DateTime.Now.AddDays(20)
      };
      var content = new StringContent(
        JsonSerializer.Serialize(expectedVacation),
        Encoding.UTF8,
        "application/json"
      );

      var response = await HttpClient.PutAsync("Employee/1/vacation/100", content);

      response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async void ShouldReturnNotFoundResultWhenRequestUrlParametersIsIncorrect()
    {
      var expectedVacation = new Vacation
      {
        Start = DateTime.Now.AddDays(10),
        End = DateTime.Now.AddDays(20)
      };
      var content = new StringContent(
        JsonSerializer.Serialize(expectedVacation),
        Encoding.UTF8,
        "application/json"
      );

      var response = await HttpClient.PutAsync("Employee/asd/vacation/def", content);

      response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async void ShouldReturnBadRequestResultWhenVacationInBodyIsIncorrect()
    {
      var expectedVacation = new Vacation();
      var content = new StringContent(
        JsonSerializer.Serialize(expectedVacation),
        Encoding.UTF8,
        "application/json"
      );

      var response = await HttpClient.PutAsync("Employee/2/vacation/1002", content);

      response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    public void Dispose()
    {
      using var connection = new SqlConnection(_connectionString);
      connection.Execute(DefaultSqlScripts.DeleteVacationTestData());
      connection.Execute(DefaultSqlScripts.DeleteEmployeeTestData());
    }
  }
}