using VacationPlanner.Constants;
using VacationPlanner.DataAccess;
using VacationPlanner.Exceptions;
using VacationPlanner.Models;

namespace VacationPlanner.Services
{
  public class TeamLeadService : ITeamLeadService
  {
    public TeamLeadService(IDbService dbService)
    {
      DbService = dbService;
    }

    private IDbService DbService { get; }

    public Vacation Approve(int teamLeadId, int vacationId)
    {
      Validate(teamLeadId, vacationId);

      var vacation = DbService.ChangeVacationState(vacationId, VacationState.Approved);
      return new Vacation(vacation.Id, vacation.Start, vacation.End, vacation.State);
    }

    public Vacation Decline(int teamLeadId, int vacationId)
    {
      Validate(teamLeadId, vacationId);

      var vacation = DbService.ChangeVacationState(vacationId, VacationState.Declined);
      return new Vacation(vacation.Id, vacation.Start, vacation.End, vacation.State);
    }

    private void Validate(int teamLeadId, int vacationId)
    {
      var employee = DbService.GetEmployee(teamLeadId);
      if (employee.Role != EmployeeRole.TeamLead)
        throw new NotAllowedActionException("Can't change vacation state because you are not teamLead");

      var vacation = DbService.GetVacation(vacationId);
      if (vacation.State != VacationState.Pending)
        throw new NotAllowedActionException(
          "Can't change vacation state because vacation is not in pending state");

      var vacationOwner = DbService.GetEmployee(vacation.EmployeeId);
      if (vacationOwner.TeamId != employee.TeamId)
        throw new NotAllowedActionException(
          "Can't change vacation state because it is a vacation of an employee from another team");
    }
  }
}