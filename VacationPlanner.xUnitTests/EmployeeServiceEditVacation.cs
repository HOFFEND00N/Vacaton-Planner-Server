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
    public class EmployeeServiceEditVacation
    {
        private DateTime currentDate;
        private EmployeeService EmployeeService;
        private StubDbService StubDbService;
        private int employeeId = 0;
        private int vacationId = 0;

        public EmployeeServiceEditVacation()
        {
            currentDate = DateTime.Now;
            StubDbService = new StubDbService(new List<DataEmployee>());
            EmployeeService =
                new EmployeeService(
                    StubDbService);
            StubDbService.Employees.Add(new DataEmployee(employeeId, "test name", new List<DataVacation>()));
            StubDbService.Employees[0].Vacations.Add(new DataVacation(vacationId, currentDate.AddDays(10),
                currentDate.AddDays(20),
                VacationState.Pending, employeeId));
        }

        [Fact]
        public void ShouldEditVacation()
        {
            var start = currentDate.AddDays(11);
            var end = currentDate.AddDays(21);
            var expectedVacation =
                new Vacation(start, end, VacationState.Approved);

            var actualVacation = EmployeeService.EditVacation(employeeId, vacationId, start,
                end, VacationState.Approved);

            actualVacation.Should().BeEquivalentTo(expectedVacation);
        }

        [Fact]
        public void ShouldThrowExceptionWhenEmployeeNotFound()
        {
            var employeeId = 10;
            Func<Vacation> editVacation = () => EmployeeService.EditVacation(employeeId, vacationId,
                currentDate.AddDays(11), currentDate.AddDays(11), VacationState.Pending);

            editVacation.Should().Throw<NotFoundException>().WithMessage($"Employee with id = {employeeId} not found");
        }

        [Fact]
        public void ShouldThrowExceptionWhenTryToChangeVacationStatusFromApproved()
        {
            var vacationId = this.vacationId + 1;
            StubDbService.Employees[0].Vacations.Add(new DataVacation(vacationId, currentDate.AddDays(10),
                currentDate.AddDays(20),
                VacationState.Approved, employeeId));

            Func<Vacation> editVacation = () => EmployeeService.EditVacation(employeeId, vacationId,
                currentDate.AddDays(10), currentDate.AddDays(20), VacationState.Pending);

            editVacation.Should().Throw<InvalidOperationException>()
                .WithMessage(
                    $"Can't change vacation state from Approved/Declined to anything else with id = {vacationId}");
        }

        [Fact]
        public void ShouldThrowExceptionWhenTryToChangeVacationStatusFromDeclined()
        {
            var vacationId = this.vacationId + 1;
            StubDbService.Employees[0].Vacations.Add(new DataVacation(vacationId, currentDate.AddDays(10),
                currentDate.AddDays(20),
                VacationState.Declined, employeeId));

            Func<Vacation> editVacation = () => EmployeeService.EditVacation(employeeId, vacationId,
                currentDate.AddDays(10), currentDate.AddDays(20), VacationState.Pending);

            editVacation.Should().Throw<InvalidOperationException>()
                .WithMessage(
                    $"Can't change vacation state from Approved/Declined to anything else with id = {vacationId}");
        }
    }
}