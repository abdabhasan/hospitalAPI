using HospitalBusinessLayer.Core;
using HospitalDataLayer.Infrastructure.DTOs.Visitor;
using Microsoft.AspNetCore.Mvc;

namespace HospitalPresentation.API.Controllers
{
    [ApiController]
    [Route("api/visitors")]
    public class VisitorController : ControllerBase
    {

        private readonly clsVisitor _visitorService;

        public VisitorController(clsVisitor visitorService)
        {
            _visitorService = visitorService;
        }


        [HttpGet("GetAllVisitors", Name = "GetAllVisitors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<VisitorDTO>>> GetAllVisitorsAsync()
        {

            try
            {
                IEnumerable<VisitorDTO> visitorsList = await _visitorService.GetAllVisitorsAsync();
                if (visitorsList == null || !visitorsList.Any())
                {
                    return NotFound("No Visitors Found!");
                }
                return Ok(visitorsList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }

        }



        [HttpGet("GetVisitorByName", Name = "GetVisitorByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<VisitorDTO>>> GetVisitorByNameAsync(string visitorName)
        {

            try
            {
                IEnumerable<VisitorDTO> visitorsList = await _visitorService.GetVisitorByNameAsync(visitorName);
                if (visitorsList == null || !visitorsList.Any())
                {
                    return NotFound("No Visitors Found!");
                }
                return Ok(visitorsList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }

        }



        [HttpDelete("DeleteVisitor/{visitorId}", Name = "DeleteVisitor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteVisitorByIdAsync(int visitorId)
        {
            try
            {
                if (visitorId <= 0)
                {
                    return BadRequest("Invalid Visitor ID.");
                }

                bool isDeleted = await _visitorService.DeleteVisitorByIdAsync(visitorId);

                if (isDeleted)
                {
                    return Ok(true);
                }
                else
                {
                    return NotFound("Visitor not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }


    }
}