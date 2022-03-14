using System;
using VacationPlanner.Constants;

namespace VacationPlanner.Models
{
    public class Vacation
    {
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public VacationState vacationState { get; set; }

        public Vacation(DateTime start, DateTime end, VacationState vacationState)
        {
            this.start = start;
            this.end = end;
            this.vacationState = vacationState;
        }
    }
}
