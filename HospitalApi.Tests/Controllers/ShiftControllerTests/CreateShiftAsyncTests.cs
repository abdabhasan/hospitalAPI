using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs.Shift;
using HospitalPresentation.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HospitalApi.Tests.Controllers.ShiftControllerTests
{
    public class CreateShiftAsyncTests
    {
        private readonly Mock<IShift> _shiftServiceMock;
        private readonly ShiftController _shiftController;
        private readonly Mock<ILogger<ShiftController>> _loggerMock;

        public CreateShiftAsyncTests()
        {
            _shiftServiceMock = new Mock<IShift>();
            _loggerMock = new Mock<ILogger<ShiftController>>();
            _shiftController = new ShiftController(_shiftServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task CreateShiftAsync_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _shiftController.ModelState.AddModelError("error", "Invalid model state");

            // Act
            var result = await _shiftController.CreateShiftAsync(new CreateShiftDTO());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid data.", badRequestResult.Value);
        }

        [Fact]
        public async Task CreateShiftAsync_ReturnsBadRequest_WhenShiftCannotBeCreated()
        {
            // Arrange
            var createShiftDto = new CreateShiftDTO();
            _shiftServiceMock.Setup(s => s.CreateShiftAsync(createShiftDto)).ReturnsAsync(false);

            // Act
            var result = await _shiftController.CreateShiftAsync(createShiftDto);

            // Assert
            var badRequestResult = Assert.IsType<StatusCodeResult>(result.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task CreateShiftAsync_ReturnsCreated_WhenShiftIsCreated()
        {
            // Arrange
            var createShiftDto = new CreateShiftDTO();
            _shiftServiceMock.Setup(s => s.CreateShiftAsync(createShiftDto)).ReturnsAsync(true);

            // Act
            var result = await _shiftController.CreateShiftAsync(createShiftDto);

            // Assert
            var createdResult = Assert.IsType<StatusCodeResult>(result.Result);
            Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
        }

        [Fact]
        public async Task CreateShiftAsync_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var createShiftDto = new CreateShiftDTO();
            _shiftServiceMock.Setup(s => s.CreateShiftAsync(createShiftDto)).ThrowsAsync(new Exception("Some error"));

            // Act
            var result = await _shiftController.CreateShiftAsync(createShiftDto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", statusCodeResult.Value);
        }



    }
}