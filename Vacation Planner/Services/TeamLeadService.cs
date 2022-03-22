using System;
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

            var vacation = DbService.GetVacation(vacationId);
            if (vacation.State != VacationState.Pending)
            {
                throw new NotAllowedActionException("Can't approve/decline vacations not in pending state");
            }

            var vacationOwner = DbService.GetEmployee(vacation.EmployeeId);

            if (vacationOwner.TeamId != employee.TeamId)
            {
                throw new NotAllowedActionException("Can't approve/decline vacations in different team");
            }

            vacation = DbService.ChangeVacationState(vacationId, VacationState.Approved);
            return new Vacation(vacation.Start, vacation.End, vacation.State);
        }
    }
}