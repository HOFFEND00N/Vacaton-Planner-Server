using System;
using VacationPlanner.Constants;
using VacationPlanner.DataAccess;
using VacationPlanner.DataAccess.Models;
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
      DataEmployee employee;
      DataVacation vacation;

      try
      {
        employee = DbService.GetEmployee(teamLeadId);
      }
      catch (InvalidOperationException)
      {
        throw new NotFoundException($"Employee with id = {teamLeadId} not found");
      }

      try
      {
        vacation = DbService.GetVacation(vacationId);
      }
      catch (InvalidOperationException)
      {
        throw new NotFoundException($"Vacation with id = {vacationId} not found");
      }

      if (employee.Role != EmployeeRole.TeamLead)
        throw new NotAllowedActionException("Can't change vacation state because you are not teamLead");

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