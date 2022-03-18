using System;
using VacationPlanner.DataAccess.Models;

namespace VacationPlanner.DataAccess
{
    public interface IDbService
    {
        DataEmployee GetEmployee(int employeeId);
        DataVacation AddVacation(int employeeId, DateTime start, DateTime end);
        DataVacation DeleteVacation(int employeeId, int vacationId);
    }
}
