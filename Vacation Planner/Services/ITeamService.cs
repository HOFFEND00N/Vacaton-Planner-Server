using System.Collections.Generic;
using VacationPlanner.Models;

namespace VacationPlanner.Services
{
    public interface ITeamService
    {
        List<Employee> GetEmployeeTeam(int employeeId);
    }
}