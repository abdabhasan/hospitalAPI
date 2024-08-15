using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HospitalDataLayer.Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HospitalBusinessLayer.Core;

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
    }
}