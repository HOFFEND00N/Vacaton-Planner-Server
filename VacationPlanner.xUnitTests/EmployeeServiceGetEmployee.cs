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
    public class EmployeeServiceGetEmployee
    {
        private EmployeeService EmployeeService;
        private StubDbService StubDbService;
        private int employeeId = 0;

        public EmployeeServiceGetEmployee()
        {
            StubDbService = new StubDbService(new List<DataEmployee>());
            EmployeeService =
                new EmployeeService(
                    StubDbService);
            StubDbService.Employees.Add(new DataEmployee(employeeId, "test name", new List<DataVacation>(),
                EmployeeRole.SoftwareEngineer, 0));
        }

        [Fact]
        public void ShouldGetEmployee()
        {
            var expectedEmployee =
                new Employee(employeeId, "test name", new List<Vacation>(), EmployeeRole.SoftwareEngineer);

            var actualEmployee = EmployeeService.GetEmployee(employeeId);

            actualEmployee.Should().BeEquivalentTo(expectedEmployee);
        }

        [Fact]
        public void ShouldThrowExceptionWhenEmployeeNotExist()
        {
            var employeeId = 1;
            Func<Employee> getEmployee = () => EmployeeService.GetEmployee(employeeId);

            getEmployee.Should().Throw<NotFoundException>().WithMessage($"Employee with id = {employeeId} not found");
        }
    }
}