using System;
using System.Collections.Generic;
using VacationPlanner.Constants;
using VacationPlanner.Models;
using Xunit;
using FluentAssertions;
using VacationPlanner.Exceptions;

namespace VacationPlanner.xUnitTests
{
    public class EmployeeAddVacation
    {
        DateTime currentDate;

        public class MockedSomeClass : ISomeClass
        {
            public Vacation AddVacation(int employeeId, DateTime start, DateTime end)
            {
                return new Vacation(start, end, VacationState.Pending);
            }

            public Employee GetEmployee(int id)
            {
                throw new NotImplementedException();
            }
        }

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
            var employee = new Employee(0, "test name", new List<Vacation>());
            employee.SomeClass = new MockedSomeClass();

            var actualVacation = employee.AddVacation(vacationStartDate, vacationEndDate);

            expectedVacation.Should().BeEquivalentTo(actualVacation);
        }

        [Fact]
        public void ShouldThrowExceptionWhenEndDateLessThanStartDate()
        {
            var vacationStartDate = currentDate.AddDays(15);
            var vacationEndDate = currentDate.AddDays(1);
            var employee = new Employee(0, "test name", new List<Vacation>());
            employee.SomeClass = new MockedSomeClass();

            Func<Vacation> action = () => employee.AddVacation(vacationStartDate, vacationEndDate);

            action.Should().Throw<InvalidVacationDatesException>();
        }

        [Fact]
        public void ShouldThrowExceptionWhenVacationOverFourYears()
        {
            var vacationStartDate = currentDate.AddDays(1);
            var vacationEndDate = currentDate.AddYears(5);
            var employee = new Employee(0, "test name", new List<Vacation>());
            employee.SomeClass = new MockedSomeClass();

            Func<Vacation> action = () => employee.AddVacation(vacationStartDate, vacationEndDate);

            action.Should().Throw<InvalidVacationDatesException>();
        }

        [Fact]
        public void ShouldThrowExceptionWhenVacationStartDateLessThanCurrentDatePlusWeek()
        {
            var employee = new Employee(0, "test name", new List<Vacation>());
            var vacationStartDate = currentDate.AddDays(6);
            employee.SomeClass = new MockedSomeClass();

            Func<Vacation> action = () => employee.AddVacation(vacationStartDate, vacationStartDate.AddDays(5));

            action.Should().Throw<InvalidVacationDatesException>();
        }

        [Fact]
        public void ShouldThrowExceptionWhenVacationStartDateGreaterThanCurrentDateOverOneYear()
        {
            var employee = new Employee(0, "test name", new List<Vacation>());
            var vacationStartDate = currentDate.AddYears(1).AddDays(1);
            employee.SomeClass = new MockedSomeClass();

            Func<Vacation> action = () => employee.AddVacation(vacationStartDate, vacationStartDate.AddDays(5));

            action.Should().Throw<InvalidVacationDatesException>();
        }
    }
}
