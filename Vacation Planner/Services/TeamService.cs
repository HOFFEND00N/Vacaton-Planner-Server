using System.Collections.Generic;
using System.Linq;
using VacationPlanner.DataAccess;
using VacationPlanner.Models;

namespace VacationPlanner.Services
{
    public class TeamService
    {
        private IDbService DbService { get; set; }

        public TeamService(IDbService dbService)
        {
            DbService = dbService;
        }

        public List<Employee> GetEmployeeTeam(int employeeId)
        {
            var employee = DbService.GetEmployee(employeeId);
            var dataTeam = DbService.GetTeamMembers(employee.TeamId);

            return dataTeam.Select(employee =>
                new Employee(employee.Id, employee.Name,
                    employee.Vacations.Select(vacation => new Vacation(vacation.Start, vacation.End, vacation.State))
                        .ToList(),
                    employee.Role)).ToList();
        }
    }
}