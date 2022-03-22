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
        private EmployeeService employeeService;
        private StubDbService StubDbService;
        private int employeeId = 0;
        private int vacationId = 0;

        public EmployeeServiceEditVacation()
        {
            currentDate = DateTime.Now;
            StubDbService = new StubDbService(new List<DataEmployee>());
            employeeService =
                new EmployeeService(
                    StubDbService);
            StubDbService.Employees.Add(new DataEmployee(employeeId, "test name", new List<DataVacation>(),
                EmployeeRole.SoftwareEngineer, 0));
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
                new Vacation(start, end);

            var actualVacation = employeeService.EditVacation(employeeId, vacationId, start,
                end);

            actualVacation.Should().BeEquivalentTo(expectedVacation);
        }

        [Fact]
        public void ShouldThrowExceptionWhenEmployeeNotFound()
        {
            var employeeId = 10;
            Func<Vacation> editVacation = () => employeeService.EditVacation(employeeId, vacationId,
                currentDate.AddDays(11), currentDate.AddDays(11));

            editVacation.Should().Throw<NotFoundException>().WithMessage($"Employee with id = {employeeId} not found");
        }

        [Fact]
        public void ShouldThrowExceptionWhenEndDateLessThanStartDate()
        {
            var vacationStartDate = currentDate.AddDays(15);
            var vacationEndDate = currentDate.AddDays(1);

            Func<Vacation> editVacation = () =>
                employeeService.EditVacation(employeeId, vacationId, vacationStartDate, vacationEndDate);

            editVacation.Should().Throw<InvalidVacationDatesException>();
        }

        [Fact]
        public void ShouldThrowExceptionWhenVacationOverFourYears()
        {
            var vacationStartDate = currentDate.AddDays(1);
            var vacationEndDate = currentDate.AddYears(5);

            Func<Vacation> editVacation = () =>
                employeeService.EditVacation(employeeId, vacationId, vacationStartDate, vacationEndDate);

            editVacation.Should().Throw<InvalidVacationDatesException>();
        }

        [Fact]
        public void ShouldThrowExceptionWhenVacationStartDateLessThanCurrentDatePlusWeek()
        {
            var vacationStartDate = currentDate.AddDays(6);

            Func<Vacation> editVacation = () =>
                employeeService.EditVacation(employeeId, vacationId, vacationStartDate, vacationStartDate.AddDays(5));

            editVacation.Should().Throw<InvalidVacationDatesException>();
        }

        [Fact]
        public void ShouldThrowExceptionWhenVacationStartDateGreaterThanCurrentDateOverOneYear()
        {
            var vacationStartDate = currentDate.AddYears(1).AddDays(1);

            Func<Vacation> editVacation = () =>
                employeeService.EditVacation(employeeId, vacationId, vacationStartDate, vacationStartDate.AddDays(5));

            editVacation.Should().Throw<InvalidVacationDatesException>();
        }
    }
}