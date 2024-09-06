using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs;
using HospitalDataLayer.Infrastructure.DTOs.InsuranceClaim;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalPresentation.API.Controllers
{
    [ApiController]
    [Route("api/insurance-claims")]
    [Authorize(Policy = "BillingPolicy")]
    public class InsuranceClaimController : ControllerBase
    {
        private readonly IInsuranceClaim _insuranceClaimService;
        private readonly ILogger<InsuranceClaimController> _logger;

        public InsuranceClaimController(IInsuranceClaim insuranceClaimService, ILogger<InsuranceClaimController> logger)
        {
            _insuranceClaimService = insuranceClaimService;
            _logger = logger;
        }

        [HttpGet("GetAllInsuranceClaims", Name = "GetAllInsuranceClaims")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<InsuranceClaimDTO>>> GetAllInsuranceClaimsAsync()
        {

            try
            {
                IEnumerable<InsuranceClaimDTO> InsuranceClaimsList = await _insuranceClaimService.GetAllInsuranceClaimsAsync();
                if (InsuranceClaimsList == null || !InsuranceClaimsList.Any())
                {
                    return NotFound("No Insurance Claims Found!");
                }
                return Ok(InsuranceClaimsList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while GETTING ALL Insurance Claims");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }

        }



        [HttpGet("GetInsuranceClaimsForPatientByPatientName", Name = "GetInsuranceClaimsForPatientByPatientName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<InsuranceClaimDTO>>> GetInsuranceClaimsForPatientByPatientNameAsync(string patientName)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(patientName))
                {
                    return BadRequest("Invalid patient name.");
                }

                IEnumerable<InsuranceClaimDTO> InsuranceClaimsList = await _insuranceClaimService.GetInsuranceClaimsForPatientByPatientNameAsync(patientName);
                if (InsuranceClaimsList == null || !InsuranceClaimsList.Any())
                {
                    return NotFound("No Insurance Claims Found!");
                }
                return Ok(InsuranceClaimsList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while GETTING Insurance Claims for patient by PatientName");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }

        }




        [HttpGet("GetInsuranceClaimsForPatientByPatientId", Name = "GetInsuranceClaimsForPatientByPatientId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<InsuranceClaimDTO>>> GetInsuranceClaimsForPatientByPatientIdAsync(int patientId)
        {

            try
            {
                IEnumerable<InsuranceClaimDTO> InsuranceClaimsList = await _insuranceClaimService.GetInsuranceClaimsForPatientByPatientIdAsync(patientId);
                if (InsuranceClaimsList == null || !InsuranceClaimsList.Any())
                {
                    return NotFound("No Insurance Claims Found!");
                }
                return Ok(InsuranceClaimsList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while GETTING Insurance Claims for PATIENT BY PatientId");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }

        }



        [HttpPost("CreateInsuranceClaim", Name = "CreateInsuranceClaim")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> CreateInsuranceClaimAsync([FromBody] CreateInsuranceClaimDTO createInsuranceClaimDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid data.");
                }

                int insuranceClaimId = await _insuranceClaimService.CreateInsuranceClaimAsync(createInsuranceClaimDto);

                if (insuranceClaimId == 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }

                return CreatedAtRoute("GetInsuranceClaimsForPatientByPatientId", new { InsuranceClaimId = insuranceClaimId }, insuranceClaimId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while CREATING Insurance Claims");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }



        [HttpDelete("DeleteInsuranceClaim/{insuranceClaimId}", Name = "DeleteInsuranceClaim")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteInsuranceClaimAsync(int insuranceClaimId)
        {
            try
            {
                if (insuranceClaimId <= 0)
                {
                    return BadRequest("Invalid InsuranceClaim ID.");
                }

                bool isDeleted = await _insuranceClaimService.DeleteInsuranceClaimAsync(insuranceClaimId);

                if (isDeleted)
                {
                    return Ok(true);
                }
                else
                {
                    return NotFound("InsuranceClaim not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while DELETING Insurance Claims");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }




    }
}