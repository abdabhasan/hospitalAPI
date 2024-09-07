using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs.Staff;
using HospitalPresentation.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HospitalApi.Tests.Controllers.StaffControllerTests
{
    public class CreateStaffAsyncTests
    {
        private readonly Mock<IStaff> _staffServiceMock;
        private readonly StaffController _staffController;
        private readonly Mock<ILogger<StaffController>> _loggerMock;

        public CreateStaffAsyncTests()
        {
            _staffServiceMock = new Mock<IStaff>();
            _loggerMock = new Mock<ILogger<StaffController>>();
            _staffController = new StaffController(_staffServiceMock.Object, _loggerMock.Object);
        }


        [Fact]
        public async Task CreateStaffAsync_ReturnsCreatedAtRoute_WhenStaffIsCreated()
        {
            // Arrange
            var createStaffDto = new CreateStaffDTO { FirstName = "John", LastName = "Doe" };
            var newStaffId = 1;
            _staffServiceMock
                .Setup(service => service.CreateStaffAsync(createStaffDto))
                .ReturnsAsync(newStaffId);  // Simulate successful creation

            // Act
            var result = await _staffController.CreateStaffAsync(createStaffDto);

            // Assert
            var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result.Result);
            Assert.Equal("GetStaffById", createdAtRouteResult.RouteName);
            Assert.Equal(newStaffId, createdAtRouteResult.RouteValues["StaffId"]);
            Assert.Equal(newStaffId, createdAtRouteResult.Value);
        }

        [Fact]
        public async Task CreateStaffAsync_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var invalidStaffDto = new CreateStaffDTO();  // Simulate invalid model state
            _staffController.ModelState.AddModelError("FullName", "Required");

            // Act
            var result = await _staffController.CreateStaffAsync(invalidStaffDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid data.", badRequestResult.Value);
        }

        [Fact]
        public async Task CreateStaffAsync_ReturnsBadRequest_WhenCreationFails()
        {
            // Arrange
            var createStaffDto = new CreateStaffDTO { FirstName = "John", LastName = "Doe" };
            _staffServiceMock
                .Setup(service => service.CreateStaffAsync(createStaffDto))
                .ReturnsAsync(0);  // Simulate creation failure

            // Act
            var result = await _staffController.CreateStaffAsync(createStaffDto);

            // Assert
            var badRequestResult = Assert.IsType<StatusCodeResult>(result.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task CreateStaffAsync_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var createStaffDto = new CreateStaffDTO { FirstName = "John", LastName = "Doe" };
            _staffServiceMock
                .Setup(service => service.CreateStaffAsync(It.IsAny<CreateStaffDTO>()))
                .ThrowsAsync(new System.Exception("Some error"));

            // Act
            var result = await _staffController.CreateStaffAsync(createStaffDto);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", objectResult.Value);
        }
    }
}