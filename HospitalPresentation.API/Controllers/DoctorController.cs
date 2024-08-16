using HospitalDataLayer.Infrastructure.DTOs.Doctor;
using Microsoft.AspNetCore.Mvc;

namespace HospitalPresentation.API.Controllers
{
    [ApiController]
    [Route("api/doctors")]
    public class DoctorController : ControllerBase
    {

        [HttpGet("GetAllDoctors", Name = "GetAllDoctors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<DoctorDTO>>> GetAllDoctorsAsync()
        {

            try
            {
                List<DoctorDTO> DoctorsList = await HospitalBusinessLayer.Core.clsDoctor.GetAllDoctorsAsync();
                if (DoctorsList == null || !DoctorsList.Any())
                {
                    return NotFound("No Doctors Found!");
                }
                return Ok(DoctorsList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }

        }


        [HttpGet("GetDoctorById", Name = "GetDoctorById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DoctorDTO>> GetDoctorByIdAsync(int doctorId)
        {

            try
            {
                DoctorDTO doctor = await HospitalBusinessLayer.Core.clsDoctor.GetDoctorByIdAsync(doctorId);
                if (doctor == null)
                {
                    return NotFound($"Doctor with Id {doctorId} NOT FOUND!");
                }
                return Ok(doctor);
            }
            catch (Exception ex)
            {
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

                int doctorId = await HospitalBusinessLayer.Core.clsDoctor.CreateDoctorAsync(createDoctorDto);

                if (doctorId == 0 || doctorId == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }

                return CreatedAtRoute("GetDoctorById", new { DoctorId = doctorId }, doctorId);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

    }
}