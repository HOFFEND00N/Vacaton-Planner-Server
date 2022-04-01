using System;
using Microsoft.AspNetCore.Mvc;
using VacationPlanner.Exceptions;
using VacationPlanner.Models;
using VacationPlanner.Services;

namespace VacationPlanner.Controllers
{
    [ApiController]
    public class VacationController : ControllerBase
    {
        private readonly IEmployeeService employeeService;

        public VacationController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        [HttpPut("employee/{employeeId:int}/vacation/{vacationId:int}")]
        public IActionResult EditVacation(int employeeId, int vacationId, [FromBody] Vacation vacation)
        {
            try
            {
                var updatedVacation =
                    employeeService.EditVacation(employeeId, vacationId, vacation.Start, vacation.End);

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
    }
}