using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs.Bill;
using HospitalPresentation.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HospitalApi.Tests.Controllers.BillControllerTests
{
    public class GetBillsForPatientByPatientIdAsyncTests
    {
        private readonly BillController _billController;
        private readonly Mock<IBill> _billServiceMock;
        private readonly Mock<ILogger<BillController>> _loggerMock;

        public GetBillsForPatientByPatientIdAsyncTests()
        {
            _billServiceMock = new Mock<IBill>();
            _loggerMock = new Mock<ILogger<BillController>>();
            _billController = new BillController(_billServiceMock.Object, _loggerMock.Object);
        }


        [Fact]
        public async Task GetBillsForPatientByPatientIdAsync_ReturnsOk_WhenBillsAreFound()
        {
            // Arrange
            int patientId = 1;
            var bills = new List<BillDTO>
        {
            new BillDTO { Id = 1, PatinetId = patientId, Amount = 100, /* Other properties */ },
            new BillDTO { Id = 2, PatinetId = patientId, Amount = 200, /* Other properties */ }
        };

            _billServiceMock.Setup(s => s.GetBillsForPatientByPatientIdAsync(patientId))
                            .ReturnsAsync(bills); // Simulate found bills.

            // Act
            var result = await _billController.GetBillsForPatientByPatientIdAsync(patientId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(bills, okResult.Value);
        }

        [Fact]
        public async Task GetBillsForPatientByPatientIdAsync_ReturnsNotFound_WhenNoBillsAreFound()
        {
            // Arrange
            int patientId = 1;

            // Scenario 1: BillsList is null
            _billServiceMock.Setup(s => s.GetBillsForPatientByPatientIdAsync(patientId))
                            .ReturnsAsync((IEnumerable<BillDTO>)null); // Simulate no bills.

            // Act
            var resultNull = await _billController.GetBillsForPatientByPatientIdAsync(patientId);

            // Assert
            var notFoundResultNull = Assert.IsType<NotFoundObjectResult>(resultNull.Result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResultNull.StatusCode);
            Assert.Equal("No Bills Found!", notFoundResultNull.Value);

            // Scenario 2: BillsList is empty
            _billServiceMock.Setup(s => s.GetBillsForPatientByPatientIdAsync(patientId))
                            .ReturnsAsync(new List<BillDTO>()); // Simulate empty list.

            // Act
            var resultEmpty = await _billController.GetBillsForPatientByPatientIdAsync(patientId);

            // Assert
            var notFoundResultEmpty = Assert.IsType<NotFoundObjectResult>(resultEmpty.Result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResultEmpty.StatusCode);
            Assert.Equal("No Bills Found!", notFoundResultEmpty.Value);
        }

        [Fact]
        public async Task GetBillsForPatientByPatientIdAsync_ReturnsBadRequest_WhenPatientIdIsInvalid()
        {
            // Arrange
            int invalidPatientId = -1;

            // Act
            var result = await _billController.GetBillsForPatientByPatientIdAsync(invalidPatientId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.Equal("Invalid Patient ID.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetBillsForPatientByPatientIdAsync_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            int patientId = 1;

            _billServiceMock.Setup(s => s.GetBillsForPatientByPatientIdAsync(patientId))
                            .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _billController.GetBillsForPatientByPatientIdAsync(patientId);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", objectResult.Value);
        }

    }
}