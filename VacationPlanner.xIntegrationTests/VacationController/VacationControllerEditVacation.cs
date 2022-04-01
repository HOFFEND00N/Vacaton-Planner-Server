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
    public class VacationControllerEditVacation
    {
        private readonly HttpClient HttpClient;

        public VacationControllerEditVacation()
        {
            HttpClient = new WebApplicationFactory<Startup>().WithWebHostBuilder(_ => { })
                .CreateClient();
        }

        [Fact]
        public async void ShouldUpdateVacation()
        {
            var expectedVacation = new Vacation
            {
                Start = DateTime.Now.AddDays(10),
                End = DateTime.Now.AddDays(20),
            };
            var content = new StringContent(
                JsonSerializer.Serialize(expectedVacation),
                Encoding.UTF8,
                "application/json"
            );

            var response = await HttpClient.PutAsync("Employee/2/vacation/1002", content);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var actualVacation = JsonConvert.DeserializeObject<Vacation>(response.Content.ReadAsStringAsync().Result);
            actualVacation.End.Should().Be(expectedVacation.End.Date);
            actualVacation.Start.Should().Be(expectedVacation.Start.Date);
            actualVacation.VacationState.Should().Be(expectedVacation.VacationState);
        }

        [Fact]
        public async void ShouldReturnNotFoundResultWhenRequestForNonExistentEmployee()
        {
            var expectedVacation = new Vacation
            {
                Start = DateTime.Now.AddDays(10),
                End = DateTime.Now.AddDays(20)
            };
            var content = new StringContent(
                JsonSerializer.Serialize(expectedVacation),
                Encoding.UTF8,
                "application/json"
            );

            var response = await HttpClient.PutAsync("Employee/100/vacation/1002", content);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async void ShouldReturnNotFoundResultWhenRequestForNonExistentVacation()
        {
            var expectedVacation = new Vacation
            {
                Start = DateTime.Now.AddDays(10),
                End = DateTime.Now.AddDays(20),
            };
            var content = new StringContent(
                JsonSerializer.Serialize(expectedVacation),
                Encoding.UTF8,
                "application/json"
            );

            var response = await HttpClient.PutAsync("Employee/1/vacation/100", content);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async void ShouldReturnNotFoundResultWhenRequestUrlParametersIsIncorrect()
        {
            var expectedVacation = new Vacation
            {
                Start = DateTime.Now.AddDays(10),
                End = DateTime.Now.AddDays(20),
            };
            var content = new StringContent(
                JsonSerializer.Serialize(expectedVacation),
                Encoding.UTF8,
                "application/json"
            );

            var response = await HttpClient.PutAsync("Employee/asd/vacation/def", content);

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

            var response = await HttpClient.PutAsync("Employee/2/vacation/1002", content);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}