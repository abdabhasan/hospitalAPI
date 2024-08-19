using HospitalDataLayer.Infrastructure.DTOs.Shift;
using Microsoft.AspNetCore.Mvc;

namespace HospitalPresentation.API.Controllers
{
    [ApiController]
    [Route("api/shifts")]
    public class ShiftController : ControllerBase
    {

        [HttpGet(" GetStaffShiftsByStaffId", Name = " GetStaffShiftsByStaffId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ShiftDTO>>> GetStaffShiftsByStaffIdAsync(int staffId)
        {

            try
            {

                if (staffId <= 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Staff Id should be bigger than zero!");
                }

                List<ShiftDTO> ShiftsList = await HospitalBusinessLayer.Core.clsShift.GetStaffShiftsByStaffIdAsync(staffId);
                if (ShiftsList == null || !ShiftsList.Any())
                {
                    return NotFound("No Shifts Found!");
                }
                return Ok(ShiftsList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }

        }




    }
}