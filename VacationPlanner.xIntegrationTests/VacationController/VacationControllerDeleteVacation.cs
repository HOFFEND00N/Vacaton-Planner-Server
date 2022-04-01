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
    public class VacationControllerDeleteVacation
    {
        private readonly HttpClient HttpClient;

        public VacationControllerDeleteVacation()
        {
            HttpClient = new WebApplicationFactory<Startup>().WithWebHostBuilder(_ => { })
                .CreateClient();
        }

        [Fact]
        public async void ShouldDeleteVacation()
        {
            var expectedVacation = new Vacation(DateTime.Now.AddDays(10), DateTime.Now.AddDays(20));
            var content = new StringContent(
                JsonSerializer.Serialize(expectedVacation),
                Encoding.UTF8,
                "application/json"
            );
            var responseOnVacationCreation = await HttpClient.PostAsync("employee/2/vacation", content);
            var createdVacation =
                JsonConvert.DeserializeObject<Vacation>(responseOnVacationCreation.Content.ReadAsStringAsync().Result);

            var responseOnVacationDelete = await HttpClient.DeleteAsync($"Employee/2/vacation/{createdVacation.Id}");

            responseOnVacationDelete.StatusCode.Should().Be(HttpStatusCode.OK);
            var deletedVacation =
                JsonConvert.DeserializeObject<Vacation>(responseOnVacationDelete.Content.ReadAsStringAsync().Result);
            deletedVacation.End.Should().Be(expectedVacation.End.Date);
            deletedVacation.Start.Should().Be(expectedVacation.Start.Date);
            deletedVacation.VacationState.Should().Be(expectedVacation.VacationState);
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
    }
}