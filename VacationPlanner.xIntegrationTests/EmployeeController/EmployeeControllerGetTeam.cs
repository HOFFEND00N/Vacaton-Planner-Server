﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using VacationPlanner.Constants;
using VacationPlanner.Models;
using Xunit;

namespace VacationPlanner.xIntegrationTests.EmployeeController
{
  public class EmployeeControllerGetTeam
  {
    private readonly HttpClient HttpClient;

    public EmployeeControllerGetTeam()
    {
      HttpClient = new WebApplicationFactory<Startup>().WithWebHostBuilder(_ => { })
        .CreateClient();
    }

    [Fact]
    public async void ShouldGetTeam()
    {
      var expectedTeam = new List<Employee>();
      expectedTeam.Add(new Employee(0, "Vasya Ivanov", new List<Vacation>(), EmployeeRole.TeamLead));
      expectedTeam.Add(new Employee(1, "Petr Petrov", new List<Vacation>(), EmployeeRole.SoftwareEngineer));
      expectedTeam[0].Vacations
        .Add(new Vacation(2, new DateTime(2020, 1, 1), new DateTime(2020, 2, 2), VacationState.Pending));
      expectedTeam[0].Vacations
        .Add(new Vacation(3, new DateTime(2022, 3, 24), new DateTime(2022, 4, 3), VacationState.Pending));

      var response = await HttpClient.GetAsync("Employee/1/team");

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
  }
}