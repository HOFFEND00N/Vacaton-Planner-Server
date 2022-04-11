using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using Dapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using VacationPlanner.Constants;
using VacationPlanner.DataAccess.Models;
using VacationPlanner.Models;
using Xunit;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace VacationPlanner.xIntegrationTests.VacationController
{
  [Collection("CollectionForSequentialTestRunning")]
  public class VacationControllerDeclineVacation : ControllerTestBase, IDisposable
  {
    private readonly List<DataVacation> _vacations;
    
    public VacationControllerDeclineVacation()
    {
      using var connection = new SqlConnection(ConnectionString);
      connection.Execute(DefaultSqlScripts.CreateEmployeeTestData());
      connection.Execute(DefaultSqlScripts.CreateVacationTestData());
      _vacations = (List<DataVacation>) connection.Query<DataVacation>(DefaultSqlScripts.SelectVacationTestData());
    }

    [Fact]
    public async void ShouldApproveVacation()
    {
      var expectedVacation = _vacations[0];
      expectedVacation.State = VacationState.Declined;
      var content = new StringContent(
        JsonSerializer.Serialize(expectedVacation),
        Encoding.UTF8,
        "application/json"
      );

      var response = await HttpClient.PutAsync($"employee/{expectedVacation.EmployeeId}/vacation/{expectedVacation.Id}/decline", content);

      response.StatusCode.Should().Be(HttpStatusCode.OK);
      var actualVacation = JsonConvert.DeserializeObject<Vacation>(response.Content.ReadAsStringAsync().Result);
      actualVacation.End.Should().Be(expectedVacation.End.Date);
      actualVacation.Start.Should().Be(expectedVacation.Start.Date);
      actualVacation.VacationState.Should().Be(expectedVacation.State);
    }

    [Fact]
    public async void ShouldReturnNotFoundResultWhenRequestForNonExistingEmployee()
    {
      var content = new StringContent("", Encoding.UTF8, "application/json");

      var response = await HttpClient.PutAsync("employee/200/vacation/1002/decline", content);

      response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async void ShouldReturnNotFoundResultWhenRequestForNonExistingVacation()
    {
      var content = new StringContent("", Encoding.UTF8, "application/json");

      var response = await HttpClient.PutAsync("employee/2/vacation/100002/decline", content);

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