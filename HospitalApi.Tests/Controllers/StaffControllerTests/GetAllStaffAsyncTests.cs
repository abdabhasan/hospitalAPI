using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs.Staff;
using HospitalPresentation.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HospitalApi.Tests.Controllers.StaffControllerTests
{
    public class GetAllStaffAsyncTests
    {
        private readonly Mock<IStaff> _staffServiceMock;
        private readonly StaffController _staffController;
        private readonly Mock<ILogger<StaffController>> _loggerMock;

        public GetAllStaffAsyncTests()
        {
            _staffServiceMock = new Mock<IStaff>();
            _loggerMock = new Mock<ILogger<StaffController>>();
            _staffController = new StaffController(_staffServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetAllStaffAsync_ReturnsOk_WhenStaffListIsNotEmpty()
        {
            // Arrange
            var staffList = new List<StaffDTO>
        {
            new StaffDTO { Id = 1, FullName = "John Doe" },
            new StaffDTO { Id = 2, FullName = "Jane Doe" }
        };
            _staffServiceMock
                .Setup(service => service.GetAllStaffAsync())
                .ReturnsAsync(staffList);  // Simulate successful retrieval

            // Act
            var result = await _staffController.GetAllStaffAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<StaffDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);  // Expect two staff members in the list
        }

        [Fact]
        public async Task GetAllStaffAsync_ReturnsNotFound_WhenNoStaffIsFound()
        {
            // Arrange
            _staffServiceMock
                .Setup(service => service.GetAllStaffAsync())
                .ReturnsAsync(new List<StaffDTO>());  // Simulate no staff found

            // Act
            var result = await _staffController.GetAllStaffAsync();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No Staff Found!", notFoundResult.Value);
        }

        [Fact]
        public async Task GetAllStaffAsync_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            _staffServiceMock
                .Setup(service => service.GetAllStaffAsync())
                .ThrowsAsync(new System.Exception("Some error"));

            // Act
            var result = await _staffController.GetAllStaffAsync();

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", objectResult.Value);
        }
    }
}