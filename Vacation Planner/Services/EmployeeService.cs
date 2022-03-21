﻿using System;
using System.Linq;
using VacationPlanner.DataAccess;
using VacationPlanner.Exceptions;
using VacationPlanner.Models;

namespace VacationPlanner.Services
{
    public class EmployeeService
    {
        private IDbService DbService { get; set; }

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
            return new Vacation(vacation.Start, vacation.End);
        }

        public Vacation DeleteVacation(int employeeId, int vacationId)
        {
            var employee = DbService.GetEmployee(employeeId);
            if (employee.Vacations.Count(vacation => vacation.Id == vacationId) != 1)
            {
                throw new NotFoundException($"Vacation with id = {vacationId} not found");
            }

            var deletedVacation = DbService.DeleteVacation(vacationId);
            return new Vacation(deletedVacation.Start, deletedVacation.End);
        }

        public Employee GetEmployee(int employeeId)
        {
            var employee = DbService.GetEmployee(employeeId);
            var vacations = employee.Vacations.Select(vacation =>
                new Vacation(vacation.Start, vacation.End)).ToList();
            return new Employee(employee.Id, employee.Name, vacations, employee.Role);
        }

        public Vacation EditVacation(int employeeId, int vacationId, DateTime start, DateTime end)
        {
            if (DateTime.Compare(start, end) > 0 || (end - start).TotalDays > 365 * 4 ||
                DateTime.Compare(start, DateTime.Now.AddDays(7)) < 0 || (start - DateTime.Now).TotalDays > 365)
            {
                throw new InvalidVacationDatesException();
            }
            var employee = DbService.GetEmployee(employeeId);
            var vacation = employee.Vacations.FirstOrDefault(vacation => vacation.Id == vacationId);
            if (vacation == null)
            {
                throw new NotFoundException($"Vacation with id = {vacationId} not found");
            }

            var updatedVacation = DbService.EditVacation(vacationId, start, end);
            return new Vacation(updatedVacation.Start, updatedVacation.End);
        }
    }
}