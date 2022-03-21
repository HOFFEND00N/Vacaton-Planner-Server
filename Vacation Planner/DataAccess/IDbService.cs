using System;
using VacationPlanner.Constants;
using VacationPlanner.DataAccess.Models;

namespace VacationPlanner.DataAccess
{
    public interface IDbService
    {
        DataEmployee GetEmployee(int employeeId);
        DataVacation AddVacation(int employeeId, DateTime start, DateTime end);
        DataVacation DeleteVacation(int vacationId);
        DataVacation EditVacation(int vacationId, DateTime start, DateTime end, VacationState state);
    }
}
