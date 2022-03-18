using System;
using System.Collections.Generic;
using System.Linq;
using VacationPlanner.Constants;
using VacationPlanner.DataAccess;
using VacationPlanner.DataAccess.Models;

namespace VacationPlanner.xUnitTests.Stubs
{
    class StubDbService : IDbService
    {
        public List<DataEmployee> Employees;

        public StubDbService(List<DataEmployee> employees)
        {
            Employees = employees;
        }

        public DataVacation AddVacation(int employeeId, DateTime start, DateTime end)
        {
            var employee = Employees.Single(employee => employee.Id == employeeId);
            var vacation = new DataVacation(employee.Vacations.Count + 1, start, end, VacationState.Pending,
                employeeId);
            employee.Vacations.Add(vacation);
            return vacation;
        }

        public DataVacation DeleteVacation(int employeeId, int vacationId)
        {
            var employee = Employees.Single(employee => employee.Id == employeeId);
            var vacation = employee.Vacations.Single(vacation => vacation.Id == vacationId);

            employee.Vacations.Remove(vacation);
            return vacation;
        }

        public DataEmployee GetEmployee(int employeeId)
        {
            return Employees.Single(employee => employee.Id == employeeId);
        }
    }
}