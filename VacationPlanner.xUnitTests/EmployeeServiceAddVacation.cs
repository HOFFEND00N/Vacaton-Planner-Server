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
  public class EmployeeServiceAddVacation
  {
    private readonly DateTime currentDate;
    private readonly int employeeId = 0;
    private readonly EmployeeService employeeService;
    private readonly StubDbService StubDbService;

    public EmployeeServiceAddVacation()
    {
      currentDate = DateTime.Now;
      StubDbService = new StubDbService(new List<DataEmployee>());
      employeeService = new EmployeeService(StubDbService);
      StubDbService.Employees.Add(new DataEmployee(employeeId, "test name", new List<DataVacation>(),
        EmployeeRole.SoftwareEngineer, 0));
    }

    [Fact]
    public void ShouldCreateVacationWhenPassCorrectData()
    {
      var vacationStartDate = currentDate.AddDays(14);
      var vacationEndDate = currentDate.AddDays(21);
      var expectedVacation = new Vacation(1, vacationStartDate, vacationEndDate, VacationState.Pending);

      var actualVacation = employeeService.AddVacation(employeeId, vacationStartDate, vacationEndDate);

      var actualVacationInStub = StubDbService.Employees.Single(employee => employee.Id == employeeId).Vacations
        .Single(vacation => DateTime.Compare(vacationStartDate, vacation.Start) == 0 &&
                            DateTime.Compare(vacationEndDate, vacation.End) == 0);

      actualVacation.Should().BeEquivalentTo(expectedVacation);
      new Vacation(actualVacationInStub.Id, actualVacationInStub.Start, actualVacationInStub.End,
        actualVacationInStub.State).Should().BeEquivalentTo(expectedVacation);
    }

    [Fact]
    public void ShouldThrowExceptionWhenEndDateLessThanStartDate()
    {
      var vacationStartDate = currentDate.AddDays(15);
      var vacationEndDate = currentDate.AddDays(1);

      Func<Vacation> addVacation =
        () => employeeService.AddVacation(employeeId, vacationStartDate, vacationEndDate);

      addVacation.Should().Throw<InvalidVacationDatesException>();
    }

    [Fact]
    public void ShouldThrowExceptionWhenVacationOverFourYears()
    {
      var vacationStartDate = currentDate.AddDays(1);
      var vacationEndDate = currentDate.AddYears(5);

      Func<Vacation> addVacation =
        () => employeeService.AddVacation(employeeId, vacationStartDate, vacationEndDate);

      addVacation.Should().Throw<InvalidVacationDatesException>();
    }

    [Fact]
    public void ShouldThrowExceptionWhenVacationStartDateLessThanCurrentDatePlusWeek()
    {
      var vacationStartDate = currentDate.AddDays(6);

      Func<Vacation> addVacation = () =>
        employeeService.AddVacation(employeeId, vacationStartDate, vacationStartDate.AddDays(5));

      addVacation.Should().Throw<InvalidVacationDatesException>();
    }

    [Fact]
    public void ShouldThrowExceptionWhenVacationStartDateGreaterThanCurrentDateOverOneYear()
    {
      var vacationStartDate = currentDate.AddYears(1).AddDays(1);

      Func<Vacation> addVacation = () =>
        employeeService.AddVacation(employeeId, vacationStartDate, vacationStartDate.AddDays(5));

      addVacation.Should().Throw<InvalidVacationDatesException>();
    }
  }
}