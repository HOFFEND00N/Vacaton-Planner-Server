using System;
using System.Net;
using System.Net.Http;
using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using VacationPlanner.Constants;
using VacationPlanner.Models;
using Xunit;
using JsonSerializer = System.Text.Json.JsonSerializer;


namespace VacationPlanner.xIntegrationTests.VacationController
{
  public class VacationControllerApproveVacation
  {
    private readonly HttpClient HttpClient;

    public VacationControllerApproveVacation()
    {
      HttpClient = new WebApplicationFactory<Startup>().WithWebHostBuilder(_ => { })
        .CreateClient();
    }

    [Fact]
    public async void ShouldApproveVacation()
    {
      var expectedVacation = new Vacation
      {
        Start = DateTime.Now.AddDays(10),
        End = DateTime.Now.AddDays(20),
        VacationState = VacationState.Approved
      };
      var content = new StringContent(
        JsonSerializer.Serialize(expectedVacation),
        Encoding.UTF8,
        "application/json"
      );
      var responseOnVacationCreation = await HttpClient.PostAsync("employee/2/vacation", content);
      var createdVacation =
        JsonConvert.DeserializeObject<Vacation>(responseOnVacationCreation.Content.ReadAsStringAsync().Result);

      var response = await HttpClient.PutAsync($"employee/2/vacation/{createdVacation.Id}/approve", content);

      response.StatusCode.Should().Be(HttpStatusCode.OK);
      var actualVacation = JsonConvert.DeserializeObject<Vacation>(response.Content.ReadAsStringAsync().Result);
      actualVacation.End.Should().Be(expectedVacation.End.Date);
      actualVacation.Start.Should().Be(expectedVacation.Start.Date);
      actualVacation.VacationState.Should().Be(expectedVacation.VacationState);

      await HttpClient.DeleteAsync($"Employee/2/vacation/{createdVacation.Id}");
    }

    [Fact]
    public async void ShouldReturnNotFoundResultWhenRequestForNonExistingEmployee()
    {
      var content = new StringContent("", Encoding.UTF8, "application/json");

      var response = await HttpClient.PutAsync("employee/200/vacation/1002/approve", content);

      response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async void ShouldReturnNotFoundResultWhenRequestForNonExistingVacation()
    {
      var content = new StringContent("", Encoding.UTF8, "application/json");

      var response = await HttpClient.PutAsync("employee/2/vacation/100002/approve", content);

      response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
  }
}