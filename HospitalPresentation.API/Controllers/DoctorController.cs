using HospitalBusinessLayer.Core;
using HospitalDataLayer.Infrastructure.DTOs.Doctor;
using Microsoft.AspNetCore.Mvc;

namespace HospitalPresentation.API.Controllers
{
    [ApiController]
    [Route("api/doctors")]
    public class DoctorController : ControllerBase
    {

        private readonly clsDoctor _doctorService;
        private readonly ILogger<DoctorController> _logger;

        public DoctorController(clsDoctor doctorService, ILogger<DoctorController> logger)
        {
            _doctorService = doctorService;
            _logger = logger;
        }


        [HttpGet("GetAllDoctors", Name = "GetAllDoctors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<DoctorDTO>>> GetAllDoctorsAsync()
        {

            try
            {
                List<DoctorDTO> DoctorsList = await _doctorService.GetAllDoctorsAsync();
                if (DoctorsList == null || !DoctorsList.Any())
                {
                    return NotFound("No Doctors Found!");
                }
                return Ok(DoctorsList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching doctors.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }

        }


        [HttpGet("GetDoctorById", Name = "GetDoctorById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<DoctorDTO>> GetDoctorByIdAsync(int doctorId)
        {

            try
            {
                DoctorDTO doctor = await _doctorService.GetDoctorByIdAsync(doctorId);
                if (doctor == null)
                {
                    return NotFound($"Doctor with Id {doctorId} NOT FOUND!");
                }
                return Ok(doctor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching doctor.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }

        }



        [HttpPost("CreateDoctor", Name = "CreateDoctor")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> CreateDoctorAsync([FromBody] CreateDoctorDTO createDoctorDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid data.");
                }

                int doctorId = await _doctorService.CreateDoctorAsync(createDoctorDto);

                if (doctorId == 0 || doctorId == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }

                return CreatedAtRoute("GetDoctorById", new { DoctorId = doctorId }, doctorId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating doctor.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }



        [HttpGet("GetDoctorOfficeNumber/{doctorId}", Name = "GetDoctorOfficeNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> GetDoctorOfficeNumberAsync(int doctorId)
        {
            try
            {
                if (doctorId <= 0)
                {
                    return BadRequest("Invalid doctor ID.");
                }

                string officeNumber = await _doctorService.GetDoctorOfficeNumberAsync(doctorId);

                if (officeNumber == "Error: Doctor not found.")
                {
                    return NotFound(officeNumber);
                }
                else if (officeNumber == "Error: Doctor has no office number.")
                {
                    return NotFound(officeNumber);
                }
                else if (officeNumber.StartsWith("Error:"))
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, officeNumber);
                }

                return Ok(officeNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching doctor's office number.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }




        [HttpPut("UpdateDoctor/{doctorId}", Name = "UpdateDoctor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> UpdateDoctorAsync(int doctorId, [FromBody] UpdateDoctorDTO UpdateDoctorDto)
        {
            try
            {
                if (doctorId <= 0 || !ModelState.IsValid)
                {
                    return BadRequest("Invalid Doctor ID.");
                }

                bool isUpdated = await _doctorService.UpdateDoctorAsync(doctorId, UpdateDoctorDto);

                if (isUpdated)
                {
                    return Ok(true);
                }
                else
                {
                    return NotFound("Doctor not found or could not be updated.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating doctor.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }




        [HttpDelete("DeleteDoctor/{doctorId}", Name = "DeleteDoctor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteDoctorAsync(int doctorId)
        {
            try
            {
                if (doctorId <= 0)
                {
                    return BadRequest("Invalid Doctor ID.");
                }

                bool isDeleted = await _doctorService.DeleteDoctorAsync(doctorId);

                if (isDeleted)
                {
                    return Ok(true);
                }
                else
                {
                    return NotFound("Doctor not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting doctor.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }


    }
}