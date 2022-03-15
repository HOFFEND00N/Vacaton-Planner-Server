using System;
using VacationPlanner.Models;

namespace VacationPlanner.DataAccess
{
    public interface IDbService
    {
        Employee GetEmployee(int id);
        Vacation AddVacation(int employeeId, DateTime start, DateTime end);
    }
}
