using System;
using System.Collections.Generic;
using System.Linq;
using VacationPlanner.DataAccess;
using VacationPlanner.DataAccess.Models;
using VacationPlanner.Exceptions;
using VacationPlanner.Models;

namespace VacationPlanner.Services
{
  public class TeamService : ITeamService
  {
    public TeamService(IDbService dbService)
    {
      DbService = dbService;
    }

    private IDbService DbService { get; }

    public List<Employee> GetEmployeeTeam(int employeeId)
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

      var dataTeam = DbService.GetTeamMembers(employee.TeamId);

      return dataTeam.Select(employee =>
        new Employee(employee.Id, employee.Name,
          employee.Vacations.Select(vacation =>
              new Vacation(vacation.Id, vacation.Start, vacation.End, vacation.State))
            .ToList(),
          employee.Role)).ToList();
    }
  }
}