using System;
using Vacation_Planner.Constants;

namespace Vacation_Planner.Models
{
    public class Vacation
    {
        public DateTime start;
        public DateTime end;
        public VacationState vacationState;

        public Vacation(DateTime start, DateTime end, VacationState vacationState)
        {
            this.start = start;
            this.end = end;
            this.vacationState = vacationState;
        }
    }
}
