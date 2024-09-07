using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs.Staff;
using HospitalPresentation.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HospitalApi.Tests.Controllers.StaffControllerTests
{
    public class UpdateStaffByIdAsyncTests
    {
        private readonly Mock<IStaff> _staffServiceMock;
        private readonly StaffController _staffController;
        private readonly Mock<ILogger<StaffController>> _loggerMock;

        public UpdateStaffByIdAsyncTests()
        {
            _staffServiceMock = new Mock<IStaff>();
            _loggerMock = new Mock<ILogger<StaffController>>();
            _staffController = new StaffController(_staffServiceMock.Object, _loggerMock.Object);
        }
        [Fact]
        public async Task UpdateStaffByIdAsync_ReturnsOk_WhenUpdateIsSuccessful()
        {
            // Arrange
            var staffId = 1;
            var updateStaffDto = new UpdateStaffDTO { FirstName = "Updated", LastName = "Name" };
            _staffServiceMock
                .Setup(service => service.UpdateStaffByIdAsync(staffId, updateStaffDto))
                .ReturnsAsync(true);  // Simulate successful update

            // Act
            var result = await _staffController.UpdateStaffByIdAsync(staffId, updateStaffDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.True((bool)okResult.Value);
        }

        [Fact]
        public async Task UpdateStaffByIdAsync_ReturnsBadRequest_WhenStaffIdIsInvalid()
        {
            // Arrange
            var invalidStaffId = 0;
            var updateStaffDto = new UpdateStaffDTO { FirstName = "Updated", LastName = "Name" };

            // Act
            var result = await _staffController.UpdateStaffByIdAsync(invalidStaffId, updateStaffDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid Staff ID.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateStaffByIdAsync_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var staffId = 1;
            var invalidStaffDto = new UpdateStaffDTO();  // Simulate invalid model state
            _staffController.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _staffController.UpdateStaffByIdAsync(staffId, invalidStaffDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid Staff ID.", badRequestResult.Value);  // Because the staffId is valid but model state fails
        }

        [Fact]
        public async Task UpdateStaffByIdAsync_ReturnsNotFound_WhenUpdateFails()
        {
            // Arrange
            var staffId = 1;
            var updateStaffDto = new UpdateStaffDTO { FirstName = "Updated", LastName = "Name" };
            _staffServiceMock
                .Setup(service => service.UpdateStaffByIdAsync(staffId, updateStaffDto))
                .ReturnsAsync(false);  // Simulate update failure (e.g., staff not found)

            // Act
            var result = await _staffController.UpdateStaffByIdAsync(staffId, updateStaffDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Staff not found or could not be updated.", notFoundResult.Value);
        }

        [Fact]
        public async Task UpdateStaffByIdAsync_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var staffId = 1;
            var updateStaffDto = new UpdateStaffDTO { FirstName = "Updated", LastName = "Name" };
            _staffServiceMock
                .Setup(service => service.UpdateStaffByIdAsync(It.IsAny<int>(), It.IsAny<UpdateStaffDTO>()))
                .ThrowsAsync(new System.Exception("Some error"));

            // Act
            var result = await _staffController.UpdateStaffByIdAsync(staffId, updateStaffDto);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", objectResult.Value);
        }



    }
}