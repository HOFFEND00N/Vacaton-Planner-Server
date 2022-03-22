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
            StubDbService.Employees.Add(new DataEmployee(employeeId, "test name", new List<DataVacation>(),
                EmployeeRole.SoftwareEngineer, 0));
            StubDbService.Employees[0].Vacations.Add(new DataVacation(0, currentDate.AddDays(10),
                currentDate.AddDays(20),
                VacationState.Pending, employeeId));
        }

        [Fact]
        public void ShouldDeleteVacation()
        {
            var expectedVacation =
                new Vacation(currentDate.AddDays(10), currentDate.AddDays(20));

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
            var employeeId = 1;
            Func<Vacation> deleteVacation = () => EmployeeService.DeleteVacation(employeeId, 0);

            deleteVacation.Should().Throw<NotFoundException>()
                .WithMessage($"Employee with id = {employeeId} not found");
        }

        [Fact]
        public void ShouldThrowExceptionWhenVacationNotExist()
        {
            var vacationId = 1;
            Func<Vacation> deleteVacation = () => EmployeeService.DeleteVacation(employeeId, vacationId);

            deleteVacation.Should().Throw<NotFoundException>()
                .WithMessage($"Vacation with id = {vacationId} not found");
        }
    }
}