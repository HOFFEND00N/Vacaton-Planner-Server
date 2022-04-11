using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using Dapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using VacationPlanner.DataAccess.Models;
using VacationPlanner.Models;
using Xunit;

namespace VacationPlanner.xIntegrationTests.EmployeeController
{
  [Collection("CollectionForSequentialTestRunning")]
  public class EmployeeControllerGetTeam : ControllerTestBase, IDisposable
  {
    private List<Employee> _employees;
    private List<DataVacation> _vacations;

    public EmployeeControllerGetTeam()
    {
      using var connection = new SqlConnection(ConnectionString);
      connection.Execute(DefaultSqlScripts.CreateEmployeeTestData());
      connection.Execute(DefaultSqlScripts.CreateVacationTestData());
      _employees = (List<Employee>) connection.Query<Employee>(DefaultSqlScripts.SelectEmployeeTestData());
      _vacations = (List<DataVacation>) connection.Query<DataVacation>(DefaultSqlScripts.SelectVacationTestData());
    }

    [Fact]
    public async void ShouldGetTeam()
    {
      var expectedTeam = new List<Employee>();
      _employees[0].Vacations = new List<Vacation>();
      _employees[0].Vacations
        .AddRange(_vacations.Where(vacation => vacation.EmployeeId == _employees[0].Id)
          .Select(vacation => new Vacation(vacation.Id, vacation.Start, vacation.End, vacation.State)));
      _employees[1].Vacations = new List<Vacation>();
      expectedTeam.Add(_employees[0]);
      expectedTeam.Add(_employees[1]);

      var response = await HttpClient.GetAsync($"Employee/{_employees[0].Id}/team");

      response.StatusCode.Should().Be(HttpStatusCode.OK);
      var team = JsonConvert.DeserializeObject<List<Employee>>(await response.Content.ReadAsStringAsync());
      team.Should().BeEquivalentTo(expectedTeam);
    }

    [Fact]
    public async void ShouldReturnNotFoundResultWhenRequestForNonExistingEmployee()
    {
      var response = await HttpClient.GetAsync("Employee/2002/team");

      response.StatusCode
        .Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async void ShouldReturnNotFoundResultWhenRequestIsIncorrect()
    {
      var response = await HttpClient.GetAsync("Employee/asd/team");

      response.StatusCode
        .Should().Be(HttpStatusCode.NotFound);
    }

    public void Dispose()
    {
      using var connection = new SqlConnection(ConnectionString);
      connection.Execute(DefaultSqlScripts.DeleteVacationTestData());
      connection.Execute(DefaultSqlScripts.DeleteEmployeeTestData());
    }
  }
}