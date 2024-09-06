using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs.Bill;
using HospitalPresentation.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HospitalApi.Tests.Controllers.BillControllerTests
{
    public class GetBillByIdAsyncTests
    {

        private readonly BillController _billController;
        private readonly Mock<IBill> _billServiceMock;
        private readonly Mock<ILogger<BillController>> _loggerMock;

        public GetBillByIdAsyncTests()
        {
            _billServiceMock = new Mock<IBill>();
            _loggerMock = new Mock<ILogger<BillController>>();
            _billController = new BillController(_billServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetBillByIdAsync_ReturnsOk_WhenBillIsFound()
        {
            // Arrange
            int billId = 1;
            var bill = new BillDTO { Id = billId, Amount = 150, /* Other properties */ };

            _billServiceMock.Setup(s => s.GetBillByIdAsync(billId))
                            .ReturnsAsync(bill); // Simulate found bill.

            // Act
            var result = await _billController.GetBillByIdAsync(billId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(bill, okResult.Value);
        }


        [Fact]
        public async Task GetBillByIdAsync_ReturnsNotFound_WhenBillIsNotFound()
        {
            // Arrange
            int billId = 1;

            _billServiceMock.Setup(s => s.GetBillByIdAsync(billId))
                            .ReturnsAsync((BillDTO)null); // Simulate no bill found.

            // Act
            var result = await _billController.GetBillByIdAsync(billId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.Equal($"Bill with Id {billId} NOT FOUND!", notFoundResult.Value);
        }


        [Fact]
        public async Task GetBillByIdAsync_ReturnsBadRequest_WhenBillIdIsInvalid()
        {
            // Arrange
            int invalidBillId = -1;

            // Act
            var result = await _billController.GetBillByIdAsync(invalidBillId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }


        [Fact]
        public async Task GetBillByIdAsync_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            int billId = 1;

            _billServiceMock.Setup(s => s.GetBillByIdAsync(billId))
                            .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _billController.GetBillByIdAsync(billId);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", objectResult.Value);
        }


    }
}