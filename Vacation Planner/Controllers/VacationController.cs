using System;
using Microsoft.AspNetCore.Mvc;
using VacationPlanner.Exceptions;
using VacationPlanner.Models;
using VacationPlanner.Services;

namespace VacationPlanner.Controllers
{
  [ApiController]
  [Route("employee/{employeeId:int}/vacation")]
  public class VacationController : ControllerBase
  {
    private readonly IEmployeeService _employeeService;
    private readonly ITeamLeadService _teamLeadService;

    public VacationController(IEmployeeService employeeService, ITeamLeadService teamLeadService)
    {
      _employeeService = employeeService;
      _teamLeadService = teamLeadService;
    }

    [HttpPut("{vacationId:int}")]
    public IActionResult EditVacation(int employeeId, int vacationId, [FromBody] Vacation vacation)
    {
      try
      {
        var updatedVacation =
          _employeeService.EditVacation(employeeId, vacationId, vacation.Start, vacation.End);

        return Ok(updatedVacation);
      }
      catch (NotFoundException e)
      {
        return NotFound(e.Message);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpPost]
    public IActionResult AddVacation(int employeeId, [FromBody] Vacation vacation)
    {
      try
      {
        var newVacation = _employeeService.AddVacation(employeeId, vacation.Start, vacation.End);

        return Ok(newVacation);
      }
      catch (NotFoundException e)
      {
        return NotFound(e.Message);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpDelete("{vacationId:int}")]
    public IActionResult DeleteVacation(int employeeId, int vacationId)
    {
      try
      {
        var deletedVacation = _employeeService.DeleteVacation(employeeId, vacationId);

        return Ok(deletedVacation);
      }
      catch (NotFoundException e)
      {
        return NotFound(e.Message);
      }
    }

    [HttpPut("{vacationId:int}/approve")]
    public IActionResult ApproveVacation(int employeeId, int vacationId)
    {
      try
      {
        var approvedVacation = _teamLeadService.Approve(employeeId, vacationId);

        return Ok(approvedVacation);
      }
      catch (NotFoundException e)
      {
        return NotFound(e.Message);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpPut("{vacationId:int}/decline")]
    public IActionResult DeclineVacation(int employeeId, int vacationId)
    {
      try
      {
        var declinedVacation = _teamLeadService.Decline(employeeId, vacationId);

        return Ok(declinedVacation);
      }
      catch (NotFoundException e)
      {
        return NotFound(e.Message);
      }
      catch (Exception e)
      {
        return BadRequest(e.Message);
      }
    }
  }
}