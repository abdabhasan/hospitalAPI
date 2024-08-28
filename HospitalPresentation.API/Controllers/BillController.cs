using HospitalBusinessLayer.Core;
using HospitalDataLayer.Infrastructure.DTOs.Bill;
using Microsoft.AspNetCore.Mvc;

namespace HospitalPresentation.API.Controllers
{
    [ApiController]
    [Route("api/bills")]
    public class BillController : ControllerBase
    {

        private readonly clsBill _billService;
        public BillController(clsBill billService)
        {
            _billService = billService;
        }


        [HttpPost("CreateBill", Name = "CreateBill")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

                if (billId == 0 || billId == null)
                {
                    return StatusCode(StatusCodes.Status400BadRequest);
                }

                // return CreatedAtRoute("GetBillById", new { BillId = billId }, billId);
                return CreatedAtRoute("CreateBill", new { BillId = billId }, billId);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

    }
}