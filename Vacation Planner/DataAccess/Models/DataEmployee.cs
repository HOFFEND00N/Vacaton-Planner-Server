using System.Collections.Generic;
using VacationPlanner.Constants;

namespace VacationPlanner.DataAccess.Models
{
    public class DataEmployee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<DataVacation> Vacations { get; set; }
        public int TeamId { get; set; }
        public IDbService DbHelper { get; set; }
        public EmployeeRole Role { get; }

        public DataEmployee(int id, string name, List<DataVacation> vacations, EmployeeRole role, int teamId)
        {
            Id = id;
            Name = name;
            Vacations = vacations;
            Role = role;
            TeamId = teamId;
        }
    }
}