using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
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
            var response = await HttpClient.GetAsync("Employee/1/team");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var team = JsonConvert.DeserializeObject<List<Employee>>(await response.Content.ReadAsStringAsync());
        }
        
        [Fact]
        public async void ShouldReturnNotFoundResultWhenRequestForNonExistingEmployee()
        {
            var response = await HttpClient.GetAsync("Employee/2/team");

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