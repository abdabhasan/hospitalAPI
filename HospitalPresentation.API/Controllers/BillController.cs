using HospitalBusinessLayer.Core;
using HospitalDataLayer.Infrastructure.DTOs.Bill;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalPresentation.API.Controllers
{
    [ApiController]
    [Route("api/bills")]
    [Authorize(Policy = "BillingPolicy")]
    public class BillController : ControllerBase
    {

        private readonly clsBill _billService;
        private readonly ILogger<BillController> _logger;
        public BillController(clsBill billService, ILogger<BillController> logger)
        {
            _billService = billService;
            _logger = logger;
        }


        [HttpPost("CreateBill", Name = "CreateBill")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> CreateBillAsync([FromBody] CreateBillDTO createBillDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid data.");
                }

                int billId = await _billService.CreateBillAsync(createBillDto);

                if (billId == 0)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }

                // return CreatedAtRoute("GetBillById", new { BillId = billId }, billId);
                return CreatedAtRoute("CreateBill", new { BillId = billId }, billId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while CREATING bill");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }



        [HttpDelete("DeleteBill/{billId}", Name = "DeleteBill")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteBillAsync(int billId)
        {
            try
            {
                if (billId <= 0)
                {
                    return BadRequest("Invalid bill ID.");
                }

                bool isDeleted = await _billService.DeleteBillAsyncById(billId);

                if (isDeleted)
                {
                    return Ok(true);
                }
                else
                {
                    return NotFound("Bill not found or could not be deleted.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while DELETING bill");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }



        [HttpGet("GetAllBills", Name = "GetAllBills")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<BillDTO>>> GetAllBillsAsync()
        {

            try
            {
                IEnumerable<BillDTO> BillsList = await _billService.GetAllBillsAsync();
                if (BillsList == null || !BillsList.Any())
                {
                    return NotFound("No Bills Found!");
                }
                return Ok(BillsList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while GETTING ALL bills");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }

        }



        [HttpGet("GetBillById", Name = "GetBillById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BillDTO>> GetBillByIdAsync(int BillId)
        {

            try
            {
                BillDTO Bill = await _billService.GetBillByIdAsync(BillId);
                if (Bill == null)
                {
                    return NotFound($"Bill with Id {BillId} NOT FOUND!");
                }
                return Ok(Bill);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while GETTING bill by Id");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }

        }



        [HttpGet("GetBillsForPatientByPatientId", Name = "GetBillsForPatientByPatientId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<BillDTO>>> GetBillsForPatientByPatientIdAsync(int patientId)
        {

            try
            {
                IEnumerable<BillDTO> BillsList = await _billService.GetBillsForPatientByPatientIdAsync(patientId);
                if (BillsList == null || !BillsList.Any())
                {
                    return NotFound("No Bills Found!");
                }
                return Ok(BillsList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while GETTING bills for PATIENT BY PATIENT ID");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }

        }



        [HttpGet("GetBillsForPatientByPatientName", Name = "GetBillsForPatientByPatientName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<BillDTO>>> GetBillsForPatientByPatientNameAsync(string patientName)
        {

            try
            {
                IEnumerable<BillDTO> BillsList = await _billService.GetBillsForPatientByPatientNameAsync(patientName);
                if (BillsList == null || !BillsList.Any())
                {
                    return NotFound("No Insurance Claims Found!");
                }
                return Ok(BillsList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while GETTING bills for PATIENT BY PATIENT NAME");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }

        }


    }
}