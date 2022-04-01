using System;
using System.Net;
using System.Net.Http;
using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using VacationPlanner.Models;
using Xunit;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace VacationPlanner.xIntegrationTests.VacationController
{
    public class VacationControllerAddVacation
    {
        private readonly HttpClient HttpClient;

        public VacationControllerAddVacation()
        {
            HttpClient = new WebApplicationFactory<Startup>().WithWebHostBuilder(_ => { })
                .CreateClient();
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

            var response = await HttpClient.PostAsync("employee/0/vacation", content);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var actualVacation = JsonConvert.DeserializeObject<Vacation>(response.Content.ReadAsStringAsync().Result);
            actualVacation.End.Should().Be(expectedVacation.End.Date);
            actualVacation.Start.Should().Be(expectedVacation.Start.Date);
            actualVacation.VacationState.Should().Be(expectedVacation.VacationState);
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
    }
}