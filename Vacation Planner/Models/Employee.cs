using System;
using System.Collections.Generic;
using Vacation_Planner.Exceptions;

namespace Vacation_Planner.Models
{
    public class Employee
    {
        public string name;
        public List<Vacation> Vacations;

        public Employee(string name, List<Vacation> vacations)
        {
            this.name = name;
            Vacations = vacations;
        }

        public Vacation AddVacation(DateTime start, DateTime end)
        {
            if (DateTime.Compare(start, end) > 0 || (end - start).TotalDays > 365 * 4 ||
                DateTime.Compare(start, DateTime.Now.AddDays(7)) < 0 || (start - DateTime.Now).TotalDays > 365)
            {
                throw new InvalidVacationDatesException();
            }
            return new Vacation(start, end, Constants.VacationState.Pending);
        }
    }
}
