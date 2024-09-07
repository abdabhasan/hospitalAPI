using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs.User;
using HospitalPresentation.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HospitalApi.Tests.Controllers.UserControllerTests
{
    public class UpdateUserAsyncTests
    {
        private readonly Mock<IUser> _userServiceMock;
        private readonly UserController _userController;
        private readonly Mock<ILogger<UserController>> _loggerMock;

        public UpdateUserAsyncTests()
        {
            _userServiceMock = new Mock<IUser>();
            _loggerMock = new Mock<ILogger<UserController>>();
            _userController = new UserController(_userServiceMock.Object, _loggerMock.Object);
        }
        [Fact]
        public async Task UpdateUserAsync_InvalidUserId_ReturnsBadRequest()
        {
            // Arrange
            int invalidUserId = 0;
            var updateUserDto = new UpdateUserDTO { Username = "John", RoleId = 2 };

            // Act
            var result = await _userController.UpdateUserAsync(invalidUserId, updateUserDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid User ID.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateUserAsync_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            int userId = 1;
            var invalidUpdateUserDto = new UpdateUserDTO(); // Empty or invalid DTO
            _userController.ModelState.AddModelError("FirstName", "First name is required");

            // Act
            var result = await _userController.UpdateUserAsync(userId, invalidUpdateUserDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid User ID.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateUserAsync_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            int userId = 1;
            var updateUserDto = new UpdateUserDTO { Username = "John", RoleId = 2 };
            _userServiceMock.Setup(service => service.UpdateUserAsync(userId, updateUserDto)).ReturnsAsync(false);

            // Act
            var result = await _userController.UpdateUserAsync(userId, updateUserDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("User not found or could not be updated.", notFoundResult.Value);
        }

        [Fact]
        public async Task UpdateUserAsync_ValidUpdate_ReturnsOk()
        {
            // Arrange
            int userId = 1;
            var updateUserDto = new UpdateUserDTO { Username = "John", RoleId = 2 };
            _userServiceMock.Setup(service => service.UpdateUserAsync(userId, updateUserDto)).ReturnsAsync(true);

            // Act
            var result = await _userController.UpdateUserAsync(userId, updateUserDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.True((bool)okResult.Value);
        }

        [Fact]
        public async Task UpdateUserAsync_InternalServerError_Returns500()
        {
            // Arrange
            int userId = 1;
            var updateUserDto = new UpdateUserDTO { Username = "John", RoleId = 2 };
            _userServiceMock.Setup(service => service.UpdateUserAsync(userId, updateUserDto)).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _userController.UpdateUserAsync(userId, updateUserDto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", statusCodeResult.Value);
        }



    }
}