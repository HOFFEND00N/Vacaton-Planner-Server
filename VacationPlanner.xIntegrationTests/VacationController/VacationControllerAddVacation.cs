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
using VacationPlanner.Models;
using Xunit;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace VacationPlanner.xIntegrationTests.VacationController
{
  [Collection("CollectionForSequentialTestRunning")]
  public class VacationControllerAddVacation : ControllerTestBase, IDisposable
  {
    private readonly List<Employee> _employees;

    public VacationControllerAddVacation()
    {
      using var connection = new SqlConnection(ConnectionString);
      connection.Execute(DefaultSqlScripts.CreateEmployeeTestData());
      _employees = (List<Employee>) connection.Query<Employee>(DefaultSqlScripts.SelectEmployeeTestData());
    }

    [Fact]
    public async void ShouldCreateVacation()
    {
      var expectedVacation = new Vacation(DateTime.Now.AddDays(10), DateTime.Now.AddDays(20));

      var content = new StringContent(
        JsonSerializer.Serialize(expectedVacation),
        Encoding.UTF8,
        "application/json"
      );

      var response = await HttpClient.PostAsync($"employee/{_employees[0].Id}/vacation", content);

      response.StatusCode.Should().Be(HttpStatusCode.OK);
      var actualVacation = JsonConvert.DeserializeObject<Vacation>(response.Content.ReadAsStringAsync().Result);
      actualVacation.End.Should().Be(expectedVacation.End.Date);
      actualVacation.Start.Should().Be(expectedVacation.Start.Date);
      actualVacation.VacationState.Should().Be(expectedVacation.VacationState);

      await HttpClient.DeleteAsync($"employee/{_employees[0].Id}/vacation/{actualVacation.Id}");
    }

    [Fact]
    public async void ShouldReturnNotFoundResultWhenRequestForNonExistentEmployee()
    {
      var expectedVacation = new Vacation(DateTime.Now.AddDays(10), DateTime.Now.AddDays(20));

      var content = new StringContent(
        JsonSerializer.Serialize(expectedVacation),
        Encoding.UTF8,
        "application/json"
      );

      var response = await HttpClient.PostAsync("employee/100/vacation", content);

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

      var response = await HttpClient.PostAsync("employee/100/vacation", content);

      response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    public void Dispose()
    {
      using var connection = new SqlConnection(ConnectionString);
      connection.Execute(DefaultSqlScripts.DeleteVacationTestData());
      connection.Execute(DefaultSqlScripts.DeleteEmployeeTestData());
    }
  }
}