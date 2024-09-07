using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs.Shift;
using HospitalPresentation.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HospitalApi.Tests.Controllers.ShiftControllerTests
{
    public class DeleteShiftByIdAsyncTests
    {
        private readonly Mock<IShift> _shiftServiceMock;
        private readonly ShiftController _shiftController;
        private readonly Mock<ILogger<ShiftController>> _loggerMock;

        public DeleteShiftByIdAsyncTests()
        {
            _shiftServiceMock = new Mock<IShift>();
            _loggerMock = new Mock<ILogger<ShiftController>>();
            _shiftController = new ShiftController(_shiftServiceMock.Object, _loggerMock.Object);
        }


        [Fact]
        public async Task DeleteShiftByIdAsync_ReturnsOk_WhenShiftIsDeleted()
        {
            // Arrange
            var doctorId = 1;
            _shiftServiceMock
                .Setup(service => service.DeleteShiftByIdAsync(doctorId))
                .ReturnsAsync(true);  // Simulate successful deletion

            // Act
            var result = await _shiftController.DeleteShiftByIdAsync(doctorId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.True((bool)okResult.Value);  // Expect true on successful deletion
        }

        [Fact]
        public async Task DeleteShiftByIdAsync_ReturnsBadRequest_WhenDoctorIdIsInvalid()
        {
            // Arrange
            var invalidDoctorId = 0;  // Invalid doctorId

            // Act
            var result = await _shiftController.DeleteShiftByIdAsync(invalidDoctorId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid Shift ID.", badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteShiftByIdAsync_ReturnsNotFound_WhenShiftDoesNotExist()
        {
            // Arrange
            var doctorId = 1;
            _shiftServiceMock
                .Setup(service => service.DeleteShiftByIdAsync(doctorId))
                .ReturnsAsync(false);  // Simulate shift not found

            // Act
            var result = await _shiftController.DeleteShiftByIdAsync(doctorId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Shift not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task DeleteShiftByIdAsync_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var doctorId = 1;
            _shiftServiceMock
                .Setup(service => service.DeleteShiftByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new System.Exception("Some error"));

            // Act
            var result = await _shiftController.DeleteShiftByIdAsync(doctorId);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", objectResult.Value);
        }

    }
}