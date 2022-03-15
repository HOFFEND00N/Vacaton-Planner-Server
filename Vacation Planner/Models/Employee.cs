using System.Collections.Generic;
using VacationPlanner.DataAccess;

namespace VacationPlanner.Models
{
    public class Employee
    {
        public string Name { get; set; }
        public List<Vacation> Vacations { get; set; }
        public IDbService DbHelper { get; set; }
    }
}
