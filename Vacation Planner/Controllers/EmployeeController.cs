using Microsoft.AspNetCore.Mvc;
using VacationPlanner.Exceptions;
using VacationPlanner.Services;

namespace VacationPlanner.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService employeeService;
        private readonly ITeamService teamService;

        public EmployeeController(IEmployeeService employeeService, ITeamService teamService)
        {
            this.employeeService = employeeService;
            this.teamService = teamService;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetEmployee(int id)
        {
            try
            {
                var employee = employeeService.GetEmployee(id);

                return Ok(employee);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("{employeeId:int}/team")]
        public IActionResult GetTeam(int employeeId)
        {
            try
            {
                var team = teamService.GetEmployeeTeam(employeeId);

                return Ok(team);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}