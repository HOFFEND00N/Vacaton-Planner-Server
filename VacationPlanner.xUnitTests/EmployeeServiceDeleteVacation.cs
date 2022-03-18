using System;
using System.Collections.Generic;
using System.Linq;
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
    public class EmployeeServiceDeleteVacation
    {
        private DateTime currentDate;
        private EmployeeService EmployeeService;
        private StubDbService StubDbService;
        private int employeeId = 0;

        public EmployeeServiceDeleteVacation()
        {
            currentDate = DateTime.Now;
            StubDbService = new StubDbService(new List<DataEmployee>());
            EmployeeService =
                new EmployeeService(
                    StubDbService);
            StubDbService.Employees.Add(new DataEmployee(employeeId, "test name", new List<DataVacation>()));
            StubDbService.Employees[0].Vacations.Add(new DataVacation(0, currentDate.AddDays(10),
                currentDate.AddDays(20),
                VacationState.Pending, employeeId));
        }

        [Fact]
        public void ShouldDeleteVacation()
        {
            var expectedVacation =
                new Vacation(currentDate.AddDays(10), currentDate.AddDays(20), VacationState.Pending);

            var actualVacation = EmployeeService.DeleteVacation(employeeId, 0);

            actualVacation.Should().BeEquivalentTo(expectedVacation);
            var employee = StubDbService.Employees.Single(employee => employee.Id == employeeId);
            Func<DataVacation> vacationSearch = () => employee.Vacations.Single(vacation => vacation.Id == 0);
            vacationSearch.Should().Throw<InvalidOperationException>()
                .WithMessage("Sequence contains no matching element");
        }

        [Fact]
        public void ShouldThrowExceptionWhenEmployeeNotExist()
        {
            Func<Vacation> deleteVacation = () => EmployeeService.DeleteVacation(1, 0);

            deleteVacation.Should().Throw<InvalidOperationException>()
                .WithMessage("Sequence contains no matching element");
        }

        [Fact]
        public void ShouldThrowExceptionWhenVacationNotExist()
        {
            Func<Vacation> deleteVacation = () => EmployeeService.DeleteVacation(employeeId, 1);

            deleteVacation.Should().Throw<VacationNotFoundException>();
        }
    }
}