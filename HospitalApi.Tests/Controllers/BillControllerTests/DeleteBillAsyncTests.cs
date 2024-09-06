using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs.Bill;
using HospitalPresentation.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HospitalApi.Tests.Controllers.BillControllerTests
{
    public class DeleteBillAsyncTests
    {
        private readonly BillController _billController;
        private readonly Mock<IBill> _billServiceMock;
        private readonly Mock<ILogger<BillController>> _loggerMock;

        public DeleteBillAsyncTests()
        {
            _billServiceMock = new Mock<IBill>();
            _loggerMock = new Mock<ILogger<BillController>>();
            _billController = new BillController(_billServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task DeleteBillAsync_ReturnsOk_WhenBillIsDeleted()
        {
            // Arrange
            int billId = 1;
            _billServiceMock.Setup(s => s.DeleteBillAsyncById(billId))
                            .ReturnsAsync(true); // Simulate successful deletion.

            // Act
            var result = await _billController.DeleteBillAsync(billId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.True((bool)okResult.Value);
        }


        [Fact]
        public async Task DeleteBillAsync_ReturnsBadRequest_WhenBillIdIsInvalid()
        {
            // Arrange
            int invalidBillId = -1;

            // Act
            var result = await _billController.DeleteBillAsync(invalidBillId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.Equal("Invalid bill ID.", badRequestResult.Value);
        }


        [Fact]
        public async Task DeleteBillAsync_ReturnsNotFound_WhenBillIsNotDeleted()
        {
            // Arrange
            int billId = 1;
            _billServiceMock.Setup(s => s.DeleteBillAsyncById(billId))
                            .ReturnsAsync(false); // Simulate failure to delete bill (not found).

            // Act
            var result = await _billController.DeleteBillAsync(billId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.Equal("Bill not found or could not be deleted.", notFoundResult.Value);
        }


        [Fact]
        public async Task DeleteBillAsync_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            int billId = 1;
            _billServiceMock.Setup(s => s.DeleteBillAsyncById(billId))
                            .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _billController.DeleteBillAsync(billId);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", objectResult.Value);
        }




    }
}