using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs.Staff;
using HospitalPresentation.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HospitalApi.Tests.Controllers.StaffControllerTests
{
    public class DeleteStaffByIdAsyncTests
    {
        private readonly Mock<IStaff> _staffServiceMock;
        private readonly StaffController _staffController;
        private readonly Mock<ILogger<StaffController>> _loggerMock;

        public DeleteStaffByIdAsyncTests()
        {
            _staffServiceMock = new Mock<IStaff>();
            _loggerMock = new Mock<ILogger<StaffController>>();
            _staffController = new StaffController(_staffServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task DeleteStaffByIdAsync_ReturnsOk_WhenDeletionIsSuccessful()
        {
            // Arrange
            var staffId = 1;
            _staffServiceMock
                .Setup(service => service.DeleteStaffByIdAsync(staffId))
                .ReturnsAsync(true);  // Simulate successful deletion

            // Act
            var result = await _staffController.DeleteStaffByIdAsync(staffId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.True((bool)okResult.Value);
        }

        [Fact]
        public async Task DeleteStaffByIdAsync_ReturnsBadRequest_WhenStaffIdIsInvalid()
        {
            // Arrange
            var invalidStaffId = 0;

            // Act
            var result = await _staffController.DeleteStaffByIdAsync(invalidStaffId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid Staff ID.", badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteStaffByIdAsync_ReturnsNotFound_WhenStaffDoesNotExist()
        {
            // Arrange
            var staffId = 1;
            _staffServiceMock
                .Setup(service => service.DeleteStaffByIdAsync(staffId))
                .ReturnsAsync(false);  // Simulate staff not found

            // Act
            var result = await _staffController.DeleteStaffByIdAsync(staffId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Staff not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task DeleteStaffByIdAsync_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var staffId = 1;
            _staffServiceMock
                .Setup(service => service.DeleteStaffByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new System.Exception("Some error"));

            // Act
            var result = await _staffController.DeleteStaffByIdAsync(staffId);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", objectResult.Value);
        }



    }
}