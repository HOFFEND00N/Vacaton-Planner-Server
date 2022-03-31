﻿using System;
using VacationPlanner.Constants;

namespace VacationPlanner.Models
{
    public class Vacation
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public VacationState VacationState { get; set; }

        public Vacation()
        {
        }

        public Vacation(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public Vacation(DateTime start, DateTime end, VacationState vacationState)
        {
            Start = start;
            End = end;
            VacationState = vacationState;
        }
    }
}