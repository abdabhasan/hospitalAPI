using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs.User;
using HospitalPresentation.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HospitalApi.Tests.Controllers.UserControllerTests
{
    public class DeleteUserAsyncTests
    {
        private readonly Mock<IUser> _userServiceMock;
        private readonly UserController _userController;
        private readonly Mock<ILogger<UserController>> _loggerMock;

        public DeleteUserAsyncTests()
        {
            _userServiceMock = new Mock<IUser>();
            _loggerMock = new Mock<ILogger<UserController>>();
            _userController = new UserController(_userServiceMock.Object, _loggerMock.Object);
        }


        [Fact]
        public async Task DeleteUserAsync_InvalidUserId_ReturnsBadRequest()
        {
            // Arrange
            int invalidUserId = 0;

            // Act
            var result = await _userController.DeleteUserAsync(invalidUserId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid User ID.", badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteUserAsync_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            int userId = 1;
            _userServiceMock.Setup(service => service.DeleteUserAsync(userId)).ReturnsAsync(false);

            // Act
            var result = await _userController.DeleteUserAsync(userId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("User not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task DeleteUserAsync_ValidUserId_ReturnsOk()
        {
            // Arrange
            int validUserId = 1;
            _userServiceMock.Setup(service => service.DeleteUserAsync(validUserId)).ReturnsAsync(true);

            // Act
            var result = await _userController.DeleteUserAsync(validUserId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.True((bool)okResult.Value);
        }

        [Fact]
        public async Task DeleteUserAsync_InternalServerError_Returns500()
        {
            // Arrange
            int userId = 1;
            _userServiceMock.Setup(service => service.DeleteUserAsync(userId)).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _userController.DeleteUserAsync(userId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", statusCodeResult.Value);
        }

    }
}