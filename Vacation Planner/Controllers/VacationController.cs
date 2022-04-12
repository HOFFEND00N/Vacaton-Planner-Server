using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<VacationController> _logger;

    public VacationController(IEmployeeService employeeService, ITeamLeadService teamLeadService,
      ILogger<VacationController> logger)
    {
      _employeeService = employeeService;
      _teamLeadService = teamLeadService;
      _logger = logger;

      _logger.LogTrace($"{nameof(VacationController)} created");
    }

    [HttpPut("{vacationId:int}")]
    public IActionResult EditVacation(int employeeId, int vacationId, [FromBody] Vacation vacation)
    {
      try
      {
        _logger.LogTrace($"Updating vacation with Id = {vacationId} for employee with id = {employeeId}");

        var updatedVacation =
          _employeeService.EditVacation(employeeId, vacationId, vacation.Start, vacation.End);

        return Ok(updatedVacation);
      }
      catch (NotFoundException e)
      {
        _logger.LogError(e, e.Message);

        return NotFound(e.Message);
      }
      catch (Exception e)
      {
        _logger.LogError(e, e.Message);

        return BadRequest(e.Message);
      }
    }

    [HttpPost]
    public IActionResult AddVacation(int employeeId, [FromBody] Vacation vacation)
    {
      try
      {
        _logger.LogTrace($"Creating new vacation for employee with id = {employeeId}");

        var newVacation = _employeeService.AddVacation(employeeId, vacation.Start, vacation.End);

        return Ok(newVacation);
      }
      catch (NotFoundException e)
      {
        _logger.LogError(e, e.Message);

        return NotFound(e.Message);
      }
      catch (Exception e)
      {
        _logger.LogError(e, e.Message);

        return BadRequest(e.Message);
      }
    }

    [HttpDelete("{vacationId:int}")]
    public IActionResult DeleteVacation(int employeeId, int vacationId)
    {
      try
      {
        _logger.LogTrace($"Deleting vacation with id = {vacationId} for employee with id = {employeeId}");

        var deletedVacation = _employeeService.DeleteVacation(employeeId, vacationId);

        return Ok(deletedVacation);
      }
      catch (NotFoundException e)
      {
        _logger.LogError(e, e.Message);

        return NotFound(e.Message);
      }
    }

    [HttpPut("{vacationId:int}/approve")]
    public IActionResult ApproveVacation(int employeeId, int vacationId)
    {
      try
      {
        _logger.LogTrace($"Approving vacation with id = {vacationId} for employee with id = {employeeId}");

        var approvedVacation = _teamLeadService.Approve(employeeId, vacationId);

        return Ok(approvedVacation);
      }
      catch (NotFoundException e)
      {
        _logger.LogError(e, e.Message);

        return NotFound(e.Message);
      }
      catch (Exception e)
      {
        _logger.LogError(e, e.Message);

        return BadRequest(e.Message);
      }
    }

    [HttpPut("{vacationId:int}/decline")]
    public IActionResult DeclineVacation(int employeeId, int vacationId)
    {
      try
      {
        _logger.LogTrace($"Declining vacation with id = {vacationId} for employee with id = {employeeId}");

        var declinedVacation = _teamLeadService.Decline(employeeId, vacationId);

        return Ok(declinedVacation);
      }
      catch (NotFoundException e)
      {
        _logger.LogError(e, e.Message);

        return NotFound(e.Message);
      }
      catch (Exception e)
      {
        _logger.LogError(e, e.Message);

        return BadRequest(e.Message);
      }
    }
  }
}