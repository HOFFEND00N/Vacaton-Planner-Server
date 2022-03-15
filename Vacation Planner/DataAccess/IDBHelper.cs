using System;
using VacationPlanner.Models;

namespace VacationPlanner.DataAccess
{
    public interface IDbHelper
    {
        Employee GetEmployee(int id);
        Vacation AddVacation(int employeeId, DateTime start, DateTime end);
    }
}
