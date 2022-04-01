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
        private readonly IEmployeeService employeeService;

        public VacationController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        [HttpPut("{vacationId:int}")]
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


        [HttpPost]
        public IActionResult AddVacation(int employeeId, [FromBody] Vacation vacation)
        {
            try
            {
                var newVacation = employeeService.AddVacation(employeeId, vacation.Start, vacation.End);

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
    }
}