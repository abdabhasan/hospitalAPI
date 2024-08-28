using HospitalBusinessLayer.Core;
using HospitalDataLayer.Infrastructure.DTOs.Shift;
using Microsoft.AspNetCore.Mvc;

namespace HospitalPresentation.API.Controllers
{
    [ApiController]
    [Route("api/shifts")]
    public class ShiftController : ControllerBase
    {


        private readonly clsShift _shiftService;
        private readonly ILogger<ShiftController> _logger;

        public ShiftController(clsShift shiftService, ILogger<ShiftController> logger)
        {
            _shiftService = shiftService;
            _logger = logger;
        }



        [HttpGet("GetAllShifts", Name = "GetAllShifts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ShiftDTO>>> GetAllShiftsAsync()
        {

            try
            {
                List<ShiftDTO> ShiftsList = await _shiftService.GetAllShiftsAsync();
                if (ShiftsList == null || !ShiftsList.Any())
                {
                    return NotFound("No Shifts Found!");
                }
                return Ok(ShiftsList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while GETTING ALL SHIFTS");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }

        }



        [HttpGet(" GetStaffShiftsByStaffId", Name = " GetStaffShiftsByStaffId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ShiftDTO>>> GetStaffShiftsByStaffIdAsync(int staffId)
        {

            try
            {

                if (staffId <= 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "Staff Id should be bigger than zero!");
                }

                List<ShiftDTO> ShiftsList = await _shiftService.GetStaffShiftsByStaffIdAsync(staffId);
                if (ShiftsList == null || !ShiftsList.Any())
                {
                    return NotFound("No Shifts Found!");
                }
                return Ok(ShiftsList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while GETTING SHIFTS BY STAFF ID");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }

        }



        [HttpPost("CreateShift", Name = "CreateShift")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> CreateShiftAsync([FromBody] CreateShiftDTO createShiftDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid data.");
                }

                bool shiftId = await _shiftService.CreateShiftAsync(createShiftDto);

                if (shiftId == false || shiftId == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }

                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while CREATING SHIFT");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }




        [HttpPut("UpdateShift", Name = "UpdateShift")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateShiftByIdAsync(int shiftId, [FromBody] UpdateShiftDTO updateShiftDto)
        {
            try
            {
                if (shiftId <= 0 || !ModelState.IsValid)
                {
                    return BadRequest("Invalid Shift ID.");
                }


                bool updateResult = await _shiftService.UpdateShiftByIdAsync(shiftId, updateShiftDto);

                if (!updateResult)
                {
                    return BadRequest("Failed to update shift.");
                }

                return Ok("Shift updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while UPDATING SHIFT");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }



        [HttpDelete("DeleteShift/{doctorId}", Name = "DeleteShift")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteShiftByIdAsync(int doctorId)
        {
            try
            {
                if (doctorId <= 0)
                {
                    return BadRequest("Invalid Shift ID.");
                }

                bool isDeleted = await _shiftService.DeleteShiftByIdAsync(doctorId);

                if (isDeleted)
                {
                    return Ok(true);
                }
                else
                {
                    return NotFound("Shift not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while DELETING SHIFT");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

    }
}