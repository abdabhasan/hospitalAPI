using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs.Bill;
using HospitalPresentation.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HospitalApi.Tests.Controllers.BillControllerTests
{
    public class GetBillsForPatientByPatientNameAsyncTests
    {
        private readonly BillController _billController;
        private readonly Mock<IBill> _billServiceMock;
        private readonly Mock<ILogger<BillController>> _loggerMock;

        public GetBillsForPatientByPatientNameAsyncTests()
        {
            _billServiceMock = new Mock<IBill>();
            _loggerMock = new Mock<ILogger<BillController>>();
            _billController = new BillController(_billServiceMock.Object, _loggerMock.Object);
        }



        [Fact]
        public async Task GetBillsForPatientByPatientNameAsync_ReturnsOk_WhenBillsAreFound()
        {
            // Arrange
            string patientName = "John Doe";
            int patinetId = 1;
            var bills = new List<BillDTO>
    {
        new BillDTO { Id = 1, PatinetId = patinetId, Amount = 100, /* Other properties */ },
        new BillDTO { Id = 2, PatinetId = patinetId, Amount = 200, /* Other properties */ }
    };

            _billServiceMock.Setup(s => s.GetBillsForPatientByPatientNameAsync(patientName))
                            .ReturnsAsync(bills); // Simulate found bills.

            // Act
            var result = await _billController.GetBillsForPatientByPatientNameAsync(patientName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(bills, okResult.Value);
        }


        [Fact]
        public async Task GetBillsForPatientByPatientNameAsync_ReturnsNotFound_WhenNoBillsAreFound()
        {
            // Arrange
            string patientName = "John Doe";

            // Scenario 1: BillsList is null
            _billServiceMock.Setup(s => s.GetBillsForPatientByPatientNameAsync(patientName))
                            .ReturnsAsync((IEnumerable<BillDTO>)null); // Simulate no bills.

            // Act
            var resultNull = await _billController.GetBillsForPatientByPatientNameAsync(patientName);

            // Assert
            var notFoundResultNull = Assert.IsType<NotFoundObjectResult>(resultNull.Result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResultNull.StatusCode);
            Assert.Equal("No Insurance Claims Found!", notFoundResultNull.Value);

            // Scenario 2: BillsList is empty
            _billServiceMock.Setup(s => s.GetBillsForPatientByPatientNameAsync(patientName))
                            .ReturnsAsync(new List<BillDTO>()); // Simulate empty list.

            // Act
            var resultEmpty = await _billController.GetBillsForPatientByPatientNameAsync(patientName);

            // Assert
            var notFoundResultEmpty = Assert.IsType<NotFoundObjectResult>(resultEmpty.Result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResultEmpty.StatusCode);
            Assert.Equal("No Insurance Claims Found!", notFoundResultEmpty.Value);
        }


        [Fact]
        public async Task GetBillsForPatientByPatientNameAsync_ReturnsBadRequest_WhenPatientNameIsInvalid()
        {
            // Arrange
            string invalidPatientName = null; // Or use string.Empty or whitespace for other scenarios

            // Act
            var result = await _billController.GetBillsForPatientByPatientNameAsync(invalidPatientName);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.Equal("Invalid patient name.", badRequestResult.Value);
        }


        [Fact]
        public async Task GetBillsForPatientByPatientNameAsync_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            string patientName = "John Doe";

            _billServiceMock.Setup(s => s.GetBillsForPatientByPatientNameAsync(patientName))
                            .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _billController.GetBillsForPatientByPatientNameAsync(patientName);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", objectResult.Value);
        }


    }
}