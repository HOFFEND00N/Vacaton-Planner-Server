using VacationPlanner.Models;

namespace VacationPlanner.Services
{
  public interface ITeamLeadService
  {
    Vacation Approve(int teamLeadId, int vacationId);
    Vacation Decline(int teamLeadId, int vacationId);
  }
}