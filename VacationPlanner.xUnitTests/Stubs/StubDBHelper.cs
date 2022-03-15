using System;
using VacationPlanner.Constants;
using VacationPlanner.DataAccess;
using VacationPlanner.Models;

namespace VacationPlanner.xUnitTests.Stubs
{
    class StubDbHelper : IDbService
    {
        public Vacation AddVacation(int employeeId, DateTime start, DateTime end)
        {
            return new Vacation(start, end, VacationState.Pending);
        }

        public Employee GetEmployee(int id)
        {
            throw new NotImplementedException();
        }
    }
}
