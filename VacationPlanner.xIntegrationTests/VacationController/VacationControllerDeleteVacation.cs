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
  public class VacationControllerDeleteVacation : ControllerTestBase, IDisposable
  {
    private readonly List<DataVacation> _vacations;

    public VacationControllerDeleteVacation()
    {
      using var connection = new SqlConnection(ConnectionString);
      connection.Execute(DefaultSqlScripts.CreateEmployeeTestData());
      connection.Execute(DefaultSqlScripts.CreateVacationTestData());
      _vacations = (List<DataVacation>) connection.Query<DataVacation>(DefaultSqlScripts.SelectVacationTestData());
    }

    [Fact]
    public async void ShouldDeleteVacation()
    {
      var expectedVacation = _vacations[0];
      var content = new StringContent(
        JsonSerializer.Serialize(expectedVacation),
        Encoding.UTF8,
        "application/json"
      );

      var deleteVacation =
        await HttpClient.DeleteAsync($"Employee/{expectedVacation.EmployeeId}/vacation/{expectedVacation.Id}");

      deleteVacation.StatusCode.Should().Be(HttpStatusCode.OK);
      var deletedVacation =
        JsonConvert.DeserializeObject<Vacation>(deleteVacation.Content.ReadAsStringAsync().Result);
      deletedVacation.End.Should().Be(expectedVacation.End.Date);
      deletedVacation.Start.Should().Be(expectedVacation.Start.Date);
      deletedVacation.VacationState.Should().Be(expectedVacation.State);
    }

    [Fact]
    public async void ShouldReturnNotFoundResultWhenRequestForNonExistingEmployee()
    {
      var response = await HttpClient.DeleteAsync("Employee/110/vacation/0");

      response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async void ShouldReturnNotFoundResultWhenRequestForNonExistingVacation()
    {
      var response = await HttpClient.DeleteAsync("Employee/0/vacation/110");

      response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    public void Dispose()
    {
      using var connection = new SqlConnection(ConnectionString);
      connection.Execute(DefaultSqlScripts.DeleteVacationTestData());
      connection.Execute(DefaultSqlScripts.DeleteEmployeeTestData());
    }
  }
}