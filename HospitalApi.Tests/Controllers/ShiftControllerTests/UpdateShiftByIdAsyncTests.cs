using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs.Shift;
using HospitalPresentation.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HospitalApi.Tests.Controllers.ShiftControllerTests
{
    public class UpdateShiftByIdAsyncTests
    {
        private readonly Mock<IShift> _shiftServiceMock;
        private readonly ShiftController _shiftController;
        private readonly Mock<ILogger<ShiftController>> _loggerMock;

        public UpdateShiftByIdAsyncTests()
        {
            _shiftServiceMock = new Mock<IShift>();
            _loggerMock = new Mock<ILogger<ShiftController>>();
            _shiftController = new ShiftController(_shiftServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task UpdateShiftByIdAsync_ReturnsOk_WhenUpdateIsSuccessful()
        {
            // Arrange
            var shiftId = 1;
            var updateShiftDto = new UpdateShiftDTO { Date = DateTime.Now };
            _shiftServiceMock
                .Setup(service => service.UpdateShiftByIdAsync(shiftId, updateShiftDto))
                .ReturnsAsync(true);

            // Act
            var result = await _shiftController.UpdateShiftByIdAsync(shiftId, updateShiftDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Shift updated successfully.", okResult.Value);
        }

        [Fact]
        public async Task UpdateShiftByIdAsync_ReturnsBadRequest_WhenShiftIdIsInvalid()
        {
            // Arrange
            var invalidShiftId = 0;  // Invalid shiftId
            var updateShiftDto = new UpdateShiftDTO { Date = DateTime.Now };

            // Act
            var result = await _shiftController.UpdateShiftByIdAsync(invalidShiftId, updateShiftDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid Shift ID.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateShiftByIdAsync_ReturnsBadRequest_WhenUpdateFails()
        {
            // Arrange
            var shiftId = 1;
            var updateShiftDto = new UpdateShiftDTO { Date = DateTime.Now };
            _shiftServiceMock
                .Setup(service => service.UpdateShiftByIdAsync(shiftId, updateShiftDto))
                .ReturnsAsync(false);  // Simulate update failure

            // Act
            var result = await _shiftController.UpdateShiftByIdAsync(shiftId, updateShiftDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Failed to update shift.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateShiftByIdAsync_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var shiftId = 1;
            var updateShiftDto = new UpdateShiftDTO { Date = DateTime.Now };
            _shiftServiceMock
                .Setup(service => service.UpdateShiftByIdAsync(It.IsAny<int>(), It.IsAny<UpdateShiftDTO>()))
                .ThrowsAsync(new System.Exception("Some error"));

            // Act
            var result = await _shiftController.UpdateShiftByIdAsync(shiftId, updateShiftDto);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", objectResult.Value);
        }


    }
}