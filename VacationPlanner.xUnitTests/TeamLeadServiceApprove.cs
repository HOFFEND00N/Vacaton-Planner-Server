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
    public class TeamLeadServiceApprove
    {
        private StubDbService StubDbService;
        private DateTime currentDate;
        private int employeeId = 0;
        private int vacationId = 0;
        private TeamLeadService teamLeadService;

        public TeamLeadServiceApprove()
        {
            currentDate = DateTime.Now;
            StubDbService = new StubDbService(new List<DataEmployee>());
            teamLeadService = new TeamLeadService(StubDbService);
            StubDbService.Employees.Add(new DataEmployee(employeeId, "test name", new List<DataVacation>(),
                EmployeeRole.SoftwareEngineer));
        }

        [Fact]
        public void ShouldApproveVacation()
        {
            var expectedVacation =
                new Vacation(currentDate.AddDays(11), currentDate.AddDays(21), VacationState.Approved);

            var actualVacation = teamLeadService.Approve(employeeId, vacationId);

            actualVacation.Should().BeEquivalentTo(expectedVacation);
        }
    }
}