using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using VacationPlanner.Exceptions;

namespace VacationPlanner.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Vacation> Vacations { get; set; }
        public int TeamId { get; set; }
        public ISomeClass SomeClass { get; set; }

        public Employee(int id, string name, List<Vacation> vacations)
        {
            Id = id;
            Name = name;
            Vacations = vacations;
        }

        public Employee(int id, string name, int teamId)
        {
            Id = id;
            Name = name;
            TeamId = teamId;
        }

        public Vacation AddVacation(DateTime start, DateTime end)
        {
            if (DateTime.Compare(start, end) > 0 || (end - start).TotalDays > 365 * 4 ||
                DateTime.Compare(start, DateTime.Now.AddDays(7)) < 0 || (start - DateTime.Now).TotalDays > 365)
            {
                throw new InvalidVacationDatesException();
            }
            return SomeClass.AddVacation(Id, start, end);
        }
    }
}
