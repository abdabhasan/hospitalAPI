using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HospitalDataLayer.Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HospitalBusinessLayer.Core;
using HospitalDataLayer.Infrastructure.DTOs.Patient;

namespace HospitalPresentation.API.Controllers
{
    [ApiController]
    [Route("api/patients")]
    public class PatientController : ControllerBase
    {


        [HttpGet("GetAllPatients", Name = "GetAllPatients")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<PatientDTO>>> GetAllPatientsAsync()
        {

            try
            {
                List<PatientDTO> PatientsList = await HospitalBusinessLayer.Core.clsPatient.GetAllPatientsAsync();
                if (PatientsList == null || !PatientsList.Any())
                {
                    return NotFound("No Patients Found!");
                }
                return Ok(PatientsList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }

        }


        [HttpGet("GetPatientById", Name = "GetPatientById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PatientDTO>> GetPatientByIdAsync(int PatientId)
        {

            try
            {
                PatientDTO Patient = await HospitalBusinessLayer.Core.clsPatient.GetPatientByIdAsync(PatientId);
                if (Patient == null)
                {
                    return NotFound($"Patient with Id {PatientId} NOT FOUND!");
                }
                return Ok(Patient);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }

        }


        [HttpPost("CreatePatient", Name = "CreatePatient")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> CreatePatientAsync([FromBody] CreatePatientDTO createPatientDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid data.");
                }

                int patientId = await HospitalBusinessLayer.Core.clsPatient.CreatePatientAsync(createPatientDto);

                return CreatedAtRoute("GetPatientById", new { PatientId = patientId }, patientId);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }


        [HttpDelete("DeletePatient/{patientId}", Name = "DeletePatient")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeletePatientAsync(int patientId)
        {
            try
            {
                if (patientId <= 0)
                {
                    return BadRequest("Invalid patient ID.");
                }

                bool isDeleted = await HospitalBusinessLayer.Core.clsPatient.DeletePatientAsync(patientId);

                if (isDeleted)
                {
                    return Ok(true);
                }
                else
                {
                    return NotFound("Patient not found or could not be deleted.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

    }
}