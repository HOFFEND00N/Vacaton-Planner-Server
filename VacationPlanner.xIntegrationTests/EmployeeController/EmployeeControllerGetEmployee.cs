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
    public class EmployeeControllerGetEmployee
    {
        private readonly HttpClient HttpClient;

        public EmployeeControllerGetEmployee()
        {
            HttpClient = new WebApplicationFactory<Startup>().WithWebHostBuilder(_ => { })
                .CreateClient();
        }

        [Fact]
        public async void ShouldGetEmployee()
        {
            var response = await HttpClient.GetAsync("Employee/1");

            response.StatusCode
                .Should().Be(HttpStatusCode.OK);
            var employee = JsonConvert.DeserializeObject<Employee>(await response.Content.ReadAsStringAsync());
            employee.Should()
                .BeEquivalentTo(new Employee(1, "Petr Petrov", new List<Vacation>(), EmployeeRole.SoftwareEngineer));
        }

        [Fact]
        public async void ShouldReturnNotFoundResultWhenRequestForNonExistingEmployee()
        {
            var response = await HttpClient.GetAsync("Employee/2");

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
    }
}