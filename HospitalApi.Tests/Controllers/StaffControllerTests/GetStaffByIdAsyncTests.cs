using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs.Staff;
using HospitalPresentation.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HospitalApi.Tests.Controllers.StaffControllerTests
{
    public class GetStaffByIdAsyncTests
    {
        private readonly Mock<IStaff> _staffServiceMock;
        private readonly StaffController _staffController;
        private readonly Mock<ILogger<StaffController>> _loggerMock;

        public GetStaffByIdAsyncTests()
        {
            _staffServiceMock = new Mock<IStaff>();
            _loggerMock = new Mock<ILogger<StaffController>>();
            _staffController = new StaffController(_staffServiceMock.Object, _loggerMock.Object);
        }


        [Fact]
        public async Task GetStaffByIdAsync_ReturnsOk_WhenStaffIsFound()
        {
            // Arrange
            var staffId = 1;
            var staff = new StaffDTO { Id = staffId, FullName = "John Doe" };
            _staffServiceMock
                .Setup(service => service.GetStaffByIdAsync(staffId))
                .ReturnsAsync(staff);  // Simulate finding a staff member

            // Act
            var result = await _staffController.GetStaffByIdAsync(staffId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<StaffDTO>(okResult.Value);
            Assert.Equal(staffId, returnValue.Id);
            Assert.Equal("John Doe", returnValue.FullName);
        }

        [Fact]
        public async Task GetStaffByIdAsync_ReturnsNotFound_WhenStaffIsNotFound()
        {
            // Arrange
            var staffId = 1;
            _staffServiceMock
                .Setup(service => service.GetStaffByIdAsync(staffId))
                .ReturnsAsync((StaffDTO)null);  // Simulate no staff found

            // Act
            var result = await _staffController.GetStaffByIdAsync(staffId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal($"Staff with Id {staffId} NOT FOUND!", notFoundResult.Value);
        }

        [Fact]
        public async Task GetStaffByIdAsync_ReturnsBadRequest_WhenStaffIdIsInvalid()
        {
            // Arrange
            var invalidStaffId = 0;  // Invalid staffId

            // Act
            var result = await _staffController.GetStaffByIdAsync(invalidStaffId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid Staff ID.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetStaffByIdAsync_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var staffId = 1;
            _staffServiceMock
                .Setup(service => service.GetStaffByIdAsync(It.IsAny<int>()))
                .ThrowsAsync(new System.Exception("Some error"));

            // Act
            var result = await _staffController.GetStaffByIdAsync(staffId);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", objectResult.Value);
        }
    }
}