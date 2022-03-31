using System;
using System.Collections.Generic;
using VacationPlanner.Constants;

namespace VacationPlanner.DataAccess.Models
{
    public class DataEmployeeAndDataVacation
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public List<DataVacation> Vacations { get; set; }
        public int TeamId { get; set; }
        public IDbService DbHelper { get; set; }
        public EmployeeRole Role { get; }
        
        public int? VacationId { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public VacationState? State { get; set; }
        public int? VacationEmployeeId { get; set; }
    }
}