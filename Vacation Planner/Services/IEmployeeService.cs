using System;
using VacationPlanner.Models;

namespace VacationPlanner.Services
{
  public interface IEmployeeService
  {
    Vacation AddVacation(int employeeId, DateTime start, DateTime end);
    Vacation DeleteVacation(int employeeId, int vacationId);
    Employee GetEmployee(int employeeId);
    Vacation EditVacation(int employeeId, int vacationId, DateTime start, DateTime end);
  }
}