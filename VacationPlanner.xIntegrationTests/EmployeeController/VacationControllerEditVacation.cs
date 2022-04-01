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

namespace VacationPlanner.xIntegrationTests.EmployeeController
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
                VacationState = VacationState.Pending
            };
            var content = new StringContent(
                JsonSerializer.Serialize(expectedVacation),
                Encoding.UTF8,
                "application/json"
            );

            var response = await HttpClient.PutAsync("Employee/2/vacation/1002", content);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var vacation = JsonConvert.DeserializeObject<Vacation>(response.Content.ReadAsStringAsync().Result);
            vacation.End.Should().Be(expectedVacation.End.Date);
            vacation.Start.Should().Be(expectedVacation.Start.Date);
            vacation.VacationState.Should().Be(expectedVacation.VacationState);
        }
    }
}