using System;
using VacationPlanner.Constants;

namespace VacationPlanner.Models
{
  public class Vacation
  {
    public Vacation()
    {
    }

    public Vacation(DateTime start, DateTime end)
    {
      Start = start;
      End = end;
    }

    public Vacation(int id, DateTime start, DateTime end)
    {
      Start = start;
      End = end;
      Id = id;
    }

    public Vacation(DateTime start, DateTime end, VacationState vacationState)
    {
      Start = start;
      End = end;
      VacationState = vacationState;
    }

    public Vacation(int id, DateTime start, DateTime end, VacationState vacationState)
    {
      Id = id;
      Start = start;
      End = end;
      VacationState = vacationState;
    }

    public int Id { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public VacationState VacationState { get; set; }
  }
}