using System;
using VacationPlanner.Constants;

namespace VacationPlanner.DataAccess.Models
{
  public class DataVacation
  {
    public DataVacation()
    {
    }

    public DataVacation(int id, DateTime start, DateTime end, VacationState state, int employeeId)
    {
      Start = start;
      End = end;
      State = state;
      EmployeeId = employeeId;
      Id = id;
    }

    public int Id { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public VacationState State { get; set; }
    public int EmployeeId { get; set; }
  }
}