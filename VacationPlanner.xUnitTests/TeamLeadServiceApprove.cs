using System;
using System.Collections.Generic;
using FluentAssertions;
using VacationPlanner.Constants;
using VacationPlanner.DataAccess.Models;
using VacationPlanner.Exceptions;
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
        private TeamLeadService teamLeadService;

        public TeamLeadServiceApprove()
        {
            currentDate = DateTime.Now;
            StubDbService = new StubDbService(new List<DataEmployee>());
            teamLeadService = new TeamLeadService(StubDbService);
            StubDbService.Employees.Add(new DataEmployee(0, "user 0", new List<DataVacation>(),
                EmployeeRole.SoftwareEngineer, 0));
            StubDbService.Employees[0].Vacations.Add(new DataVacation(0, currentDate.AddDays(10),
                currentDate.AddDays(20), VacationState.Pending, 0));
            StubDbService.Employees.Add(new DataEmployee(1, "user 1", new List<DataVacation>(),
                EmployeeRole.TeamLead, 0));
            
            StubDbService.Employees.Add(new DataEmployee(2, "user 2", new List<DataVacation>(),
                EmployeeRole.SoftwareEngineer, 1));
            StubDbService.Employees[2].Vacations.Add(new DataVacation(1, currentDate.AddDays(20),
                currentDate.AddDays(30), VacationState.Pending, 2));
        }

        [Fact]
        public void ShouldApproveVacation()
        {
            var expectedVacation =
                new Vacation(currentDate.AddDays(10), currentDate.AddDays(20), VacationState.Approved);

            var actualVacation = teamLeadService.Approve(1, 0);

            actualVacation.Should().BeEquivalentTo(expectedVacation);
        }

        [Fact]
        public void ShouldThrowExceptionWhenNotTeamLeadTryingToApproveVacation()
        {
            Func<Vacation> approveVacation = () => teamLeadService.Approve(0, 0);

            approveVacation.Should().Throw<NotAllowedActionException>()
                .WithMessage("Can't approve/decline other employees vacations");
        }
        
        [Fact]
        public void ShouldThrowExceptionWhenTeamLeadTryToApproveVacationFromDifferentTeam()
        {
            Func<Vacation> approveVacation = () => teamLeadService.Approve(2, 1);

            approveVacation.Should().Throw<NotAllowedActionException>()
                .WithMessage("Can't approve/decline employee vacations from different team");
        }
    }
}