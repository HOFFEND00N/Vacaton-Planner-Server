using System.Collections.Generic;
using VacationPlanner.Models;

namespace VacationPlanner.Services
{
  public interface ITeamService
  {
    IReadOnlyCollection<Employee> GetEmployeeTeam(int employeeId);
  }
}