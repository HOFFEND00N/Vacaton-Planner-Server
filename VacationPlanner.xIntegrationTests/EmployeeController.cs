using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;
using Xunit;

namespace VacationPlanner.xIntegrationTests
{
    public class EmployeeController
    {
        private readonly HttpClient HttpClient;

        public EmployeeController()
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
        }
    }
}