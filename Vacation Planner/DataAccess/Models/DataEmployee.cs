using System.Collections.Generic;

namespace VacationPlanner.DataAccess.Models
{
    public class DataEmployee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<DataVacation> Vacations { get; set; }
        public int TeamId { get; set; }
        public IDbService DbHelper { get; set; }

        public DataEmployee(int id, string name, List<DataVacation> vacations)
        {
            Id = id;
            Name = name;
            Vacations = vacations;
        }
    }
}
