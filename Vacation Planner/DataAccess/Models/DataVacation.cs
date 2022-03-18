﻿using System;
using VacationPlanner.Constants;

namespace VacationPlanner.DataAccess.Models
{
    public class DataVacation
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public VacationState VacationState { get; set; }
        public int EmployeeId { get; set; }

        public DataVacation(int id, DateTime start, DateTime end, VacationState vacationState, int employeeId)
        {
            Start = start;
            End = end;
            VacationState = vacationState;
            EmployeeId = employeeId;
            Id = id;
        }
    }
}