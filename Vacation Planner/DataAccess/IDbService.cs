using System;
using VacationPlanner.DataAccess.Models;

namespace VacationPlanner.DataAccess
{
    public interface IDbService
    {
        DataEmployee GetEmployee(int id);
        DataVacation AddVacation(int employeeId, DateTime start, DateTime end);
    }
}
