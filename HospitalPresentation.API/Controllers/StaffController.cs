using HospitalBusinessLayer.Core;
using HospitalDataLayer.Infrastructure.DTOs.Staff;
using Microsoft.AspNetCore.Mvc;

namespace HospitalPresentation.API.Controllers
{
    [ApiController]
    [Route("api/staff")]
    public class StaffController : ControllerBase
    {

        private readonly clsStaff _staffService;

        public StaffController(clsStaff staffService)
        {
            _staffService = staffService;
        }


        [HttpGet("GetAllStaff", Name = "GetAllStaff")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<StaffDTO>>> GetAllStaffAsync()
        {

            try
            {
                List<StaffDTO> StaffList = await _staffService.GetAllStaffAsync();
                if (StaffList == null || !StaffList.Any())
                {
                    return NotFound("No Staff Found!");
                }
                return Ok(StaffList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }

        }


        [HttpGet("GetStaffById", Name = "GetStaffById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StaffDTO>> GetStaffByIdAsync(int staffId)
        {

            try
            {
                StaffDTO staff = await _staffService.GetStaffByIdAsync(staffId);
                if (staff == null)
                {
                    return NotFound($"Staff with Id {staffId} NOT FOUND!");
                }
                return Ok(staff);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }

        }



        [HttpPost("CreateStaff", Name = "CreateStaff")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> CreateStaffAsync([FromBody] CreateStaffDTO createStaffDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid data.");
                }

                int staffId = await _staffService.CreateStaffAsync(createStaffDto);

                if (staffId == 0 || staffId == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }

                return CreatedAtRoute("GetStaffById", new { StaffId = staffId }, staffId);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }



        [HttpPut("UpdateStaffById/{staffId}", Name = "UpdateStaffById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> UpdateStaffByIdAsync(int staffId, [FromBody] UpdateStaffDTO UpdateStaffDto)
        {
            try
            {
                if (staffId <= 0 || !ModelState.IsValid)
                {
                    return BadRequest("Invalid Staff ID.");
                }

                bool isUpdated = await _staffService.UpdateStaffByIdAsync(staffId, UpdateStaffDto);

                if (isUpdated)
                {
                    return Ok(true);
                }
                else
                {
                    return NotFound("Staff not found or could not be updated.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }



        [HttpDelete("DeleteStaffById/{staffId}", Name = "DeleteStaffById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteStaffByIdAsync(int staffId)
        {
            try
            {
                if (staffId <= 0)
                {
                    return BadRequest("Invalid Staff ID.");
                }

                bool isDeleted = await _staffService.DeleteStaffByIdAsync(staffId);

                if (isDeleted)
                {
                    return Ok(true);
                }
                else
                {
                    return NotFound("Staff not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

    }
}