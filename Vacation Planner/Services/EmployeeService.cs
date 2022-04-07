using System;
using System.Linq;
using VacationPlanner.DataAccess;
using VacationPlanner.DataAccess.Models;
using VacationPlanner.Exceptions;
using VacationPlanner.Models;

namespace VacationPlanner.Services
{
  public class EmployeeService : IEmployeeService
  {
    public EmployeeService(IDbService dbService)
    {
      DbService = dbService;
    }

    private IDbService DbService { get; }

    public Vacation AddVacation(int employeeId, DateTime start, DateTime end)
    {
      ValidateVacationDates(start, end);
      try
      {
        DbService.GetEmployee(employeeId);
      }
      catch (InvalidOperationException)
      {
        throw new NotFoundException($"Employee with id = {employeeId} not found");
      }

      var vacation = DbService.AddVacation(employeeId, start, end);
      return new Vacation(vacation.Id, vacation.Start, vacation.End);
    }

    public Vacation DeleteVacation(int employeeId, int vacationId)
    {
      DataEmployee employee;
      try
      {
        employee = DbService.GetEmployee(employeeId);
      }
      catch (InvalidOperationException)
      {
        throw new NotFoundException($"Employee with id = {employeeId} not found");
      }

      if (employee.Vacations.Any(vacation => vacation.Id == vacationId) == false)
      {
        throw new NotFoundException($"Vacation with id = {vacationId} not found");
      }

      var deletedVacation = DbService.DeleteVacation(vacationId);
      return new Vacation(deletedVacation.Id, deletedVacation.Start, deletedVacation.End);
    }

    public Employee GetEmployee(int employeeId)
    {
      DataEmployee employee;
      try
      {
        employee = DbService.GetEmployee(employeeId);
      }
      catch (InvalidOperationException)
      {
        throw new NotFoundException($"Employee with id = {employeeId} not found");
      }

      var vacations = employee.Vacations.Select(vacation =>
        new Vacation(vacation.Id, vacation.Start, vacation.End)).ToList();
      return new Employee(employee.Id, employee.Name, vacations, employee.Role);
    }

    public Vacation EditVacation(int employeeId, int vacationId, DateTime start, DateTime end)
    {
      ValidateVacationDates(start, end);
      DataEmployee employee;
      try
      {
        employee = DbService.GetEmployee(employeeId);
      }
      catch (InvalidOperationException)
      {
        throw new NotFoundException($"Employee with id = {employeeId} not found");
      }

      var vacation = employee.Vacations.FirstOrDefault(vacation => vacation.Id == vacationId);
      if (vacation == null)
      {
        throw new NotFoundException($"Vacation with id = {vacationId} not found");
      }

      var updatedVacation = DbService.EditVacation(vacationId, start, end);
      return new Vacation(updatedVacation.Id, updatedVacation.Start, updatedVacation.End);
    }

    private void ValidateVacationDates(DateTime start, DateTime end)
    {
      if (DateTime.Compare(start, end) > 0 || (end - start).TotalDays > 365 * 4 ||
          DateTime.Compare(start, DateTime.Now.AddDays(7)) < 0 || (start - DateTime.Now).TotalDays > 365)
        throw new InvalidVacationDatesException();
    }
  }
}