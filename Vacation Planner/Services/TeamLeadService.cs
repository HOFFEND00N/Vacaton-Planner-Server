using VacationPlanner.Constants;
using VacationPlanner.DataAccess;
using VacationPlanner.Exceptions;
using VacationPlanner.Models;

namespace VacationPlanner.Services
{
    public class TeamLeadService
    {
        private IDbService DbService { get; set; }

        public TeamLeadService(IDbService dbService)
        {
            DbService = dbService;
        }

        public Vacation Approve(int employeeId, int vacationId)
        {
            var employee = DbService.GetEmployee(employeeId);
            if (employee.Role != EmployeeRole.TeamLead)
            {
                throw new NotAllowedActionException("Can't approve/decline other employees vacations");
            }

            var vacation = DbService.ChangeVacationState(vacationId, VacationState.Approved);
            return new Vacation(vacation.Start, vacation.End, vacation.State);
        }
    }
}