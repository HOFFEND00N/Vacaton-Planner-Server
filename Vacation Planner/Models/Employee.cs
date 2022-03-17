using System.Collections.Generic;

namespace VacationPlanner.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Vacation> Vacations { get; set; }
    }
}
