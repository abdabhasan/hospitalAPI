using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs.Bill;
using HospitalPresentation.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HospitalApi.Tests.Controllers.BillControllerTests
{
    public class CreateBillAsyncTests
    {

        private readonly BillController _billController;
        private readonly Mock<IBill> _billServiceMock;
        private readonly Mock<ILogger<BillController>> _loggerMock;

        public CreateBillAsyncTests()
        {
            _billServiceMock = new Mock<IBill>();
            _loggerMock = new Mock<ILogger<BillController>>();
            _billController = new BillController(_billServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task CreateBillAsync_ReturnsCreatedAtRoute_WhenBillIsCreated()
        {
            // Arrange
            var createBillDto = new CreateBillDTO { /* populate DTO */ };
            _billServiceMock.Setup(s => s.CreateBillAsync(createBillDto))
                            .ReturnsAsync(1); // Simulate a valid bill ID being returned.

            // Act
            var result = await _billController.CreateBillAsync(createBillDto);

            // Assert
            var actionResult = Assert.IsType<CreatedAtRouteResult>(result.Result);
            Assert.Equal(StatusCodes.Status201Created, actionResult.StatusCode);
            Assert.Equal(1, actionResult.Value);
        }

        [Fact]
        public async Task CreateBillAsync_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _billController.ModelState.AddModelError("error", "Invalid model");

            // Act
            var result = await _billController.CreateBillAsync(new CreateBillDTO());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.Equal("Invalid data.", badRequestResult.Value);
        }


        [Fact]
        public async Task CreateBillAsync_ReturnsBadRequest_WhenBillServiceReturnsZero()
        {
            // Arrange
            var createBillDto = new CreateBillDTO { /* populate DTO */ };
            _billServiceMock.Setup(s => s.CreateBillAsync(createBillDto))
                            .ReturnsAsync(0); // Simulate a failure to create the bill.

            // Act
            var result = await _billController.CreateBillAsync(createBillDto);

            // Assert
            var badRequestResult = Assert.IsType<StatusCodeResult>(result.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }


        [Fact]
        public async Task CreateBillAsync_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var createBillDto = new CreateBillDTO { /* populate DTO */ };
            _billServiceMock.Setup(s => s.CreateBillAsync(createBillDto))
                            .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _billController.CreateBillAsync(createBillDto);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", objectResult.Value);
        }


    }
}