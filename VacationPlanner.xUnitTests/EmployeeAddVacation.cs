using System;
using System.Collections.Generic;
using Vacation_Planner.Constants;
using Vacation_Planner.Models;
using Xunit;
using FluentAssertions;
using Vacation_Planner.Exceptions;

namespace VacationPlanner.xUnitTests
{
    public class EmployeeAddVacation
    {
        DateTime currentDate;
        public EmployeeAddVacation()
        {
            currentDate = DateTime.Now;
        }


        [Fact]
        public void ShouldCreateVacationWhenPassCorrectData()
        {
            var vacationStartDate = currentDate.AddDays(14);
            var vacationEndDate = currentDate.AddDays(21);
            var expectedVacation = new Vacation(vacationStartDate, vacationEndDate, VacationState.Pending);
            var employee = new Employee("test name", new List<Vacation>());

            var actualVacation = employee.AddVacation(vacationStartDate, vacationEndDate);

            expectedVacation.Should().BeEquivalentTo(actualVacation);
        }

        [Fact]
        public void ShouldThrowExceptionWhenEndDateLessThanStartDate()
        {
            var vacationStartDate = currentDate.AddDays(15);
            var vacationEndDate = currentDate.AddDays(1);
            var employee = new Employee("test name", new List<Vacation>());

            Func<Vacation> action = () => employee.AddVacation(vacationStartDate, vacationEndDate);

            action.Should().Throw<InvalidVacationDatesException>();
        }

        [Fact]
        public void ShouldThrowExceptionWhenVacationOverFourYears()
        {
            var vacationStartDate = currentDate.AddDays(1);
            var vacationEndDate = currentDate.AddYears(5);
            var employee = new Employee("test name", new List<Vacation>());

            Func<Vacation> action = () => employee.AddVacation(vacationStartDate, vacationEndDate);

            action.Should().Throw<InvalidVacationDatesException>();
        }

        [Fact]
        public void ShouldThrowExceptionWhenVacationStartDateLessThanCurrentDatePlusWeek()
        {
            var employee = new Employee("test name", new List<Vacation>());
            var vacationStartDate = currentDate.AddDays(6);

            Func<Vacation> action = () => employee.AddVacation(vacationStartDate, vacationStartDate.AddDays(5));

            action.Should().Throw<InvalidVacationDatesException>();
        }

        [Fact]
        public void ShouldThrowExceptionWhenVacationStartDateGreaterThanCurrentDateOverOneYear()
        {
            var employee = new Employee("test name", new List<Vacation>());
            var vacationStartDate = currentDate.AddYears(1).AddDays(1);

            Func<Vacation> action = () => employee.AddVacation(vacationStartDate, vacationStartDate.AddDays(5));

            action.Should().Throw<InvalidVacationDatesException>();
        }
    }
}
