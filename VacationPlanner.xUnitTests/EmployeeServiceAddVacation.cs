using System;
using System.Collections.Generic;
using System.Linq;
using VacationPlanner.Constants;
using VacationPlanner.Models;
using Xunit;
using FluentAssertions;
using VacationPlanner.DataAccess.Models;
using VacationPlanner.Exceptions;
using VacationPlanner.Services;
using VacationPlanner.xUnitTests.Stubs;

namespace VacationPlanner.xUnitTests
{
    public class EmployeeServiceAddVacation
    {
        DateTime currentDate;
        private EmployeeService employeeService;
        private StubDbService StubDbService;
        private int employeeId = 0;

        public EmployeeServiceAddVacation()
        {
            currentDate = DateTime.Now;
            StubDbService = new StubDbService(new List<DataEmployee>());
            employeeService =
                new EmployeeService(
                    StubDbService);
            StubDbService.Employees.Add(new DataEmployee(employeeId, "test name", new List<DataVacation>()));
        }

        [Fact]
        public void ShouldCreateVacationWhenPassCorrectData()
        {
            var vacationStartDate = currentDate.AddDays(14);
            var vacationEndDate = currentDate.AddDays(21);
            var expectedVacation = new Vacation(vacationStartDate, vacationEndDate, VacationState.Pending);

            var actualVacation = employeeService.AddVacation(employeeId, vacationStartDate, vacationEndDate);

            var actualVacationInStub = StubDbService.Employees.Single(employee => employee.Id == employeeId).Vacations
                .Single(vacation => DateTime.Compare(vacationStartDate, vacation.Start) == 0 &&
                                    DateTime.Compare(vacationEndDate, vacation.End) == 0);

            expectedVacation.Should().BeEquivalentTo(actualVacation);
            expectedVacation.Should().BeEquivalentTo(new Vacation(actualVacationInStub.Start, actualVacationInStub.End,
                actualVacationInStub.State));
        }

        [Fact]
        public void ShouldThrowExceptionWhenEndDateLessThanStartDate()
        {
            var vacationStartDate = currentDate.AddDays(15);
            var vacationEndDate = currentDate.AddDays(1);

            Func<Vacation> action = () => employeeService.AddVacation(employeeId, vacationStartDate, vacationEndDate);

            action.Should().Throw<InvalidVacationDatesException>();
        }

        [Fact]
        public void ShouldThrowExceptionWhenVacationOverFourYears()
        {
            var vacationStartDate = currentDate.AddDays(1);
            var vacationEndDate = currentDate.AddYears(5);

            Func<Vacation> action = () => employeeService.AddVacation(employeeId, vacationStartDate, vacationEndDate);

            action.Should().Throw<InvalidVacationDatesException>();
        }

        [Fact]
        public void ShouldThrowExceptionWhenVacationStartDateLessThanCurrentDatePlusWeek()
        {
            var vacationStartDate = currentDate.AddDays(6);

            Func<Vacation> action = () =>
                employeeService.AddVacation(employeeId, vacationStartDate, vacationStartDate.AddDays(5));

            action.Should().Throw<InvalidVacationDatesException>();
        }

        [Fact]
        public void ShouldThrowExceptionWhenVacationStartDateGreaterThanCurrentDateOverOneYear()
        {
            var vacationStartDate = currentDate.AddYears(1).AddDays(1);

            Func<Vacation> action = () =>
                employeeService.AddVacation(employeeId, vacationStartDate, vacationStartDate.AddDays(5));

            action.Should().Throw<InvalidVacationDatesException>();
        }
    }
}