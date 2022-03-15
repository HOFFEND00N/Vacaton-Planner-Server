using System.Collections.Generic;
using VacationPlanner.DataAccess;

namespace VacationPlanner.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Vacation> Vacations { get; set; }
        public int TeamId { get; set; }
        public IDbHelper DbHelper { get; }
    }
}
