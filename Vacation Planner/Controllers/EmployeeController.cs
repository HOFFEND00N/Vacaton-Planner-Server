using System;
using Microsoft.AspNetCore.Mvc;
using VacationPlanner.Services;

namespace VacationPlanner.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        [HttpGet("{id:int}")]
        public IActionResult GetEmployee(int id)
        {
            try
            {
                var employee = employeeService.GetEmployee(id);

                return Ok(employee);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}