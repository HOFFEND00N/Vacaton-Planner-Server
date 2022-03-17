using System;
using VacationPlanner.DataAccess;
using VacationPlanner.Exceptions;
using VacationPlanner.Models;

namespace VacationPlanner.Services
{
    public class EmployeeService
    {
        public IDbService DbService { get; set; }

        public EmployeeService(IDbService dbService)
        {
            DbService = dbService;
        }

        public Vacation AddVacation(int employeeId, DateTime start, DateTime end)
        {
            if (DateTime.Compare(start, end) > 0 || (end - start).TotalDays > 365 * 4 ||
                DateTime.Compare(start, DateTime.Now.AddDays(7)) < 0 || (start - DateTime.Now).TotalDays > 365)
            {
                throw new InvalidVacationDatesException();
            }

            var vacation = DbService.AddVacation(employeeId, start, end);
            return new Vacation(vacation.Start, vacation.End, vacation.VacationState);
        }
    }
}