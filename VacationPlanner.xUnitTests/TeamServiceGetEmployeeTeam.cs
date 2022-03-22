using System;
using System.Collections.Generic;
using FluentAssertions;
using VacationPlanner.Constants;
using VacationPlanner.DataAccess.Models;
using VacationPlanner.Models;
using VacationPlanner.Services;
using VacationPlanner.xUnitTests.Stubs;
using Xunit;

namespace VacationPlanner.xUnitTests
{
    public class TeamServiceGetEmployeeTeam
    {
        private TeamService teamService;
        private StubDbService StubDbService;

        public TeamServiceGetEmployeeTeam()
        {
            var currentDate = DateTime.Now;
            StubDbService = new StubDbService(new List<DataEmployee>());
            teamService =
                new TeamService(
                    StubDbService);
            StubDbService.Employees.Add(new DataEmployee(0, "user 0", new List<DataVacation>(),
                EmployeeRole.SoftwareEngineer, 0));
            StubDbService.Employees.Add(new DataEmployee(1, "user 1", new List<DataVacation>(),
                EmployeeRole.TeamLead, 0));
        }

        [Fact]
        public void ShouldGetTeam()
        {
            var expectedTeam = new List<Employee>();
            expectedTeam.Add(new Employee(0, "user 0", new List<Vacation>(), EmployeeRole.SoftwareEngineer));
            expectedTeam.Add(new Employee(1, "user 1", new List<Vacation>(), EmployeeRole.TeamLead));

            var actualTeam = teamService.GetEmployeeTeam(0);

            actualTeam.Should().BeEquivalentTo(expectedTeam);
        }
    }
}