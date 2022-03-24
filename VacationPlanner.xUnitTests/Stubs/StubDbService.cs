using System;
using System.Collections.Generic;
using System.Linq;
using VacationPlanner.Constants;
using VacationPlanner.DataAccess;
using VacationPlanner.DataAccess.Models;
using VacationPlanner.Exceptions;

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

        public DataVacation DeleteVacation(int vacationId)
        {
            foreach (var employee in Employees)
            {
                var vacation = employee.Vacations.FirstOrDefault(vacation => vacation.Id == vacationId);
                if (vacation != null)
                {
                    employee.Vacations.Remove(vacation);
                    return vacation;
                }
            }

            throw new NotFoundException($"vacation with id = {vacationId} not found");
        }

        public DataVacation EditVacation(int vacationId, DateTime start, DateTime end)
        {
            var vacation = GetVacation(vacationId);
            vacation.Start = start;
            vacation.End = end;
            return vacation;
        }

        public DataVacation ChangeVacationState(int vacationId, VacationState state)
        {
            var vacation = GetVacation(vacationId);
            vacation.State = state;
            return vacation;
        }

        public IEnumerable<DataEmployee> GetTeamMembers(int teamId)
        {
            return Employees.Where(employee => employee.TeamId == teamId);
        }

        public DataVacation GetVacation(int vacationId)
        {
            foreach (var employee in Employees)
            {
                var vacation = employee.Vacations.FirstOrDefault(vacation => vacation.Id == vacationId);
                if (vacation != null)
                {
                    return vacation;
                }
            }

            throw new NotFoundException($"vacation with id = {vacationId} not found");
        }

        public DataEmployee GetEmployee(int employeeId)
        {
            DataEmployee employee;
            try
            {
                employee = Employees.Single(employee => employee.Id == employeeId);
            }
            catch (InvalidOperationException)
            {
                throw new NotFoundException($"Employee with id = {employeeId} not found");
            }

            return employee;
        }
    }
}