using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using VacationPlanner.Exceptions;
using VacationPlanner.Resources;
using VacationPlanner.Services;

namespace VacationPlanner.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class EmployeeController : ControllerBase
  {
    private readonly IEmployeeService _employeeService;
    private readonly ITeamService _teamService;
    private readonly ILogger<EmployeeController> _logger;

    public EmployeeController(IEmployeeService employeeService, ITeamService teamService,
      ILogger<EmployeeController> logger)
    {
      _employeeService = employeeService;
      _teamService = teamService;
      _logger = logger;

      _logger.LogTrace($"{nameof(EmployeeController)} created");
    }

    [HttpGet("{id:int}")]
    public IActionResult GetEmployee(int id)
    {
      try
      {
        _logger.LogTrace($"Receiving employee with id = {id}");

        var employee = _employeeService.GetEmployee(id);

        return Ok(employee);
      }
      catch (NotFoundException e)
      {
        _logger.LogError(e, e.Message);

        return NotFound(e.Message);
      }
    }

    [HttpGet("{employeeId:int}/team")]
    public IActionResult GetTeam(int employeeId)
    {
      try
      {
        _logger.LogTrace($"Receiving employee team with employeeId = {employeeId}");

        var team = _teamService.GetEmployeeTeam(employeeId);

        return Ok(team);
      }
      catch (NotFoundException e)
      {
        _logger.LogError(e, e.Message);

        return NotFound(e.Message);
      }
    }
  }
}