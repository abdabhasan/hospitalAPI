using HospitalDataLayer.Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;
using HospitalDataLayer.Infrastructure.DTOs.Patient;
using HospitalBusinessLayer.Core;

namespace HospitalPresentation.API.Controllers
{
    [ApiController]
    [Route("api/patients")]
    public class PatientController : ControllerBase
    {


        private readonly clsPatient _patientService;
        private readonly ILogger<PatientController> _logger;

        public PatientController(clsPatient patientService, ILogger<PatientController> logger)
        {
            _patientService = patientService;
            _logger = logger;
        }


        [HttpGet("GetAllPatients", Name = "GetAllPatients")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<PatientDTO>>> GetAllPatientsAsync()
        {

            try
            {
                List<PatientDTO> PatientsList = await _patientService.GetAllPatientsAsync();
                if (PatientsList == null || !PatientsList.Any())
                {
                    return NotFound("No Patients Found!");
                }
                return Ok(PatientsList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while GETTING ALL PATIENTS");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }

        }


        [HttpGet("GetPatientById", Name = "GetPatientById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PatientDTO>> GetPatientByIdAsync(int PatientId)
        {

            try
            {
                PatientDTO Patient = await _patientService.GetPatientByIdAsync(PatientId);
                if (Patient == null)
                {
                    return NotFound($"Patient with Id {PatientId} NOT FOUND!");
                }
                return Ok(Patient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while GETTING PATIENT BY ID");
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

                int patientId = await _patientService.CreatePatientAsync(createPatientDto);

                return CreatedAtRoute("GetPatientById", new { PatientId = patientId }, patientId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while CREATING PATIENT");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }


        [HttpDelete("DeletePatient/{patientId}", Name = "DeletePatient")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeletePatientAsync(int patientId)
        {
            try
            {
                if (patientId <= 0)
                {
                    return BadRequest("Invalid patient ID.");
                }

                bool isDeleted = await _patientService.DeletePatientAsync(patientId);

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
                _logger.LogError(ex, "An error occurred while DELETING PATIENT");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }


        [HttpPut("UpdatePatient/{patientId}", Name = "UpdatePatient")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> UpdatePatientAsync(int patientId, [FromBody] UpdatePatientDTO updatePatientDto)
        {
            try
            {
                if (patientId <= 0 || !ModelState.IsValid)
                {
                    return BadRequest("Invalid patient ID or data.");
                }

                bool isUpdated = await _patientService.UpdatePatientAsync(patientId, updatePatientDto);

                if (isUpdated)
                {
                    return Ok(true);
                }
                else
                {
                    return NotFound("Patient not found or could not be updated.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while UPDATING PATIENT");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }


        [HttpGet("GetPatientMedicalHistory/{patientId}", Name = "GetPatientMedicalHistory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> GetPatientMedicalHistoryAsync(int patientId)
        {
            try
            {
                if (patientId <= 0)
                {
                    return BadRequest("Invalid patient ID.");
                }

                string medicalHistory = await _patientService.GetPatientMedicalHistoryAsync(patientId);

                if (medicalHistory == "Error: Patient not found.")
                {
                    return NotFound(medicalHistory);
                }
                else if (medicalHistory == "Error: Patient has no medical history available.")
                {
                    return NotFound(medicalHistory);
                }
                else if (medicalHistory.StartsWith("Error:"))
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, medicalHistory);
                }

                return Ok(medicalHistory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while GETTING PATIENT'S MEDICAL HISTORY");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }


        [HttpGet("GetPatientAllergies/{patientId}", Name = "GetPatientAllergies")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> GetPatientAllergiesAsync(int patientId)
        {
            try
            {
                if (patientId <= 0)
                {
                    return BadRequest("Invalid patient ID.");
                }

                string allergies = await _patientService.GetPatientAllergiesAsync(patientId);

                if (allergies == "Error: Patient not found.")
                {
                    return NotFound(allergies);
                }
                else if (allergies == "Error: Patient has no allergies.")
                {
                    return NotFound(allergies);
                }
                else if (allergies.StartsWith("Error:"))
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, allergies);
                }

                return Ok(allergies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while GETTING PATIENT'S ALLERGIES");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

    }
}