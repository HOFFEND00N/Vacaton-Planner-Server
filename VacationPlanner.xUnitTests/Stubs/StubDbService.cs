﻿using System;
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
            var vacation = new DataVacation(start, end, VacationState.Pending, employeeId);
            Employees.Single(employee => employee.Id == employeeId).Vacations.Add(vacation);
            return vacation;
        }

        public DataEmployee GetEmployee(int id)
        {
            throw new NotImplementedException();
        }
    }
}