using System;
using VacationPlanner.Constants;
using VacationPlanner.Models;
using Xunit;
using FluentAssertions;
using VacationPlanner.Exceptions;
using VacationPlanner.Services;
using VacationPlanner.xUnitTests.Stubs;

namespace VacationPlanner.xUnitTests
{
    public class EmployeeAddVacation
    {
        DateTime currentDate;
        private EmployeeService employeeService;

        public EmployeeAddVacation()
        {
            currentDate = DateTime.Now;
            employeeService = new EmployeeService();
        }


        [Fact]
        public void ShouldCreateVacationWhenPassCorrectData()
        {
            var vacationStartDate = currentDate.AddDays(14);
            var vacationEndDate = currentDate.AddDays(21);
            var expectedVacation = new Vacation(vacationStartDate, vacationEndDate, VacationState.Pending);

            var actualVacation = employeeService.AddVacation(vacationStartDate, vacationEndDate, new StubDbHelper(), 0);

            expectedVacation.Should().BeEquivalentTo(actualVacation);
        }

        [Fact]
        public void ShouldThrowExceptionWhenEndDateLessThanStartDate()
        {
            var vacationStartDate = currentDate.AddDays(15);
            var vacationEndDate = currentDate.AddDays(1);

            Func<Vacation> action = () => employeeService.AddVacation(vacationStartDate, vacationEndDate, new StubDbHelper(), 0);

            action.Should().Throw<InvalidVacationDatesException>();
        }

        [Fact]
        public void ShouldThrowExceptionWhenVacationOverFourYears()
        {
            var vacationStartDate = currentDate.AddDays(1);
            var vacationEndDate = currentDate.AddYears(5);

            Func<Vacation> action = () => employeeService.AddVacation(vacationStartDate, vacationEndDate, new StubDbHelper(), 0);

            action.Should().Throw<InvalidVacationDatesException>();
        }

        [Fact]
        public void ShouldThrowExceptionWhenVacationStartDateLessThanCurrentDatePlusWeek()
        {
            var vacationStartDate = currentDate.AddDays(6);

            Func<Vacation> action = () => employeeService.AddVacation(vacationStartDate, vacationStartDate.AddDays(5), new StubDbHelper(), 0);

            action.Should().Throw<InvalidVacationDatesException>();
        }

        [Fact]
        public void ShouldThrowExceptionWhenVacationStartDateGreaterThanCurrentDateOverOneYear()
        {
            var vacationStartDate = currentDate.AddYears(1).AddDays(1);

            Func<Vacation> action = () => employeeService.AddVacation(vacationStartDate, vacationStartDate.AddDays(5), new StubDbHelper(), 0);

            action.Should().Throw<InvalidVacationDatesException>();
        }
    }
}
