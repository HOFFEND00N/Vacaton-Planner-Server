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
    public class TeamLeadServiceDecline
    {
        private StubDbService StubDbService;
        private DateTime currentDate;
        private TeamLeadService teamLeadService;

        public TeamLeadServiceDecline()
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
        }

        [Fact]
        public void ShouldDeclineVacation()
        {
            var expectedVacation =
                new Vacation(currentDate.AddDays(10), currentDate.AddDays(20), VacationState.Declined);

            var actualVacation = teamLeadService.Decline(1, 0);

            actualVacation.Should().BeEquivalentTo(expectedVacation);
        }

        [Fact]
        public void ShouldThrowExceptionWhenNotTeamLeadTryingToDeclineVacation()
        {
            Func<Vacation> declineVacation = () => teamLeadService.Decline(0, 0);

            declineVacation.Should().Throw<NotAllowedActionException>()
                .WithMessage("Can't change vacation state because you are not teamLead");
        }

        [Fact]
        public void ShouldThrowExceptionWhenTeamLeadTryToDeclineVacationFromDifferentTeam()
        {
            StubDbService.Employees.Add(new DataEmployee(2, "user 2", new List<DataVacation>(),
                EmployeeRole.SoftwareEngineer, 1));
            StubDbService.Employees[2].Vacations.Add(new DataVacation(1, currentDate.AddDays(20),
                currentDate.AddDays(30), VacationState.Pending, 2));
            Func<Vacation> declineVacation = () => teamLeadService.Decline(1, 1);

            declineVacation.Should().Throw<NotAllowedActionException>()
                .WithMessage("Can't change vacation state because it is a vacation of an employee from another team");
        }

        [Fact]
        public void ShouldThrowExceptionWhenTeamLeadTryToDeclineVacationNotInPendingState()
        {
            StubDbService.Employees[0].Vacations.Add(new DataVacation(1, currentDate.AddDays(20),
                currentDate.AddDays(30), VacationState.Approved, 0));
            Func<Vacation> declineVacation = () => teamLeadService.Decline(1, 1);

            declineVacation.Should().Throw<NotAllowedActionException>()
                .WithMessage("Can't change vacation state because vacation is not in pending state");
        }
    }
}