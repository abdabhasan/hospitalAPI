using HospitalBusinessLayer.Core;
using HospitalDataLayer.Infrastructure.DTOs;
using HospitalDataLayer.Infrastructure.DTOs.InsuranceClaim;
using Microsoft.AspNetCore.Mvc;

namespace HospitalPresentation.API.Controllers
{
    [ApiController]
    [Route("api/insurance-claims")]
    public class InsuranceClaimController : ControllerBase
    {
        private readonly clsInsuranceClaim _insuranceClaimService;

        public InsuranceClaimController(clsInsuranceClaim insuranceClaimService)
        {
            _insuranceClaimService = insuranceClaimService;
        }

        [HttpGet("GetAllInsuranceClaims", Name = "GetAllInsuranceClaims")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }

        }



        [HttpGet("GetInsuranceClaimsForPatientByPatientName", Name = "GetInsuranceClaimsForPatientByPatientName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<InsuranceClaimDTO>>> GetInsuranceClaimsForPatientByPatientNameAsync(string patientName)
        {

            try
            {
                IEnumerable<InsuranceClaimDTO> InsuranceClaimsList = await _insuranceClaimService.GetInsuranceClaimsForPatientByPatientNameAsync(patientName);
                if (InsuranceClaimsList == null || !InsuranceClaimsList.Any())
                {
                    return NotFound("No Insurance Claims Found!");
                }
                return Ok(InsuranceClaimsList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }

        }




        [HttpGet("GetInsuranceClaimsForPatientByPatientId", Name = "GetInsuranceClaimsForPatientByPatientId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }

        }



        [HttpPost("CreateInsuranceClaim", Name = "CreateInsuranceClaim")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

                if (insuranceClaimId == 0 || insuranceClaimId == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }

                return CreatedAtRoute("GetInsuranceClaimsForPatientByPatientId", new { InsuranceClaimId = insuranceClaimId }, insuranceClaimId);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }



    }
}