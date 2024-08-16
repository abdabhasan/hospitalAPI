using HospitalDataLayer.Infrastructure.DTOs;
using Microsoft.AspNetCore.Mvc;
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


        [HttpPut("UpdatePatient/{patientId}", Name = "UpdatePatient")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> UpdatePatientAsync(int patientId, [FromBody] UpdatePatientDTO updatePatientDto)
        {
            try
            {
                if (patientId <= 0 || !ModelState.IsValid)
                {
                    return BadRequest("Invalid patient ID or data.");
                }

                bool isUpdated = await HospitalBusinessLayer.Core.clsPatient.UpdatePatientAsync(patientId, updatePatientDto);

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
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }


        [HttpGet("GetPatientMedicalHistory/{patientId}", Name = "GetPatientMedicalHistory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

                string medicalHistory = await HospitalBusinessLayer.Core.clsPatient.GetPatientMedicalHistoryAsync(patientId);

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
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }


        [HttpGet("GetPatientAllergies/{patientId}", Name = "GetPatientAllergies")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

                string allergies = await HospitalBusinessLayer.Core.clsPatient.GetPatientAllergiesAsync(patientId);

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
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

    }
}