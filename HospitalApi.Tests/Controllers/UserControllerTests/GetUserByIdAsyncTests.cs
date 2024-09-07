using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs.User;
using HospitalPresentation.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HospitalApi.Tests.Controllers.UserControllerTests
{
    public class GetUserByIdAsyncTests
    {
        private readonly Mock<IUser> _userServiceMock;
        private readonly UserController _userController;
        private readonly Mock<ILogger<UserController>> _loggerMock;

        public GetUserByIdAsyncTests()
        {
            _userServiceMock = new Mock<IUser>();
            _loggerMock = new Mock<ILogger<UserController>>();
            _userController = new UserController(_userServiceMock.Object, _loggerMock.Object);
        }
        [Fact]
        public async Task GetUserByIdAsync_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            int userId = 1;
            _userServiceMock.Setup(service => service.GetUserByIdAsync(userId)).ReturnsAsync((UserDTO)null);

            // Act
            var result = await _userController.GetUserByIdAsync(userId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal($"User with Id {userId} NOT FOUND!", notFoundResult.Value);
        }

        [Fact]
        public async Task GetUserByIdAsync_ValidUser_ReturnsOk()
        {
            // Arrange
            int userId = 1;
            var user = new UserDTO { Id = userId, Username = "John" };
            _userServiceMock.Setup(service => service.GetUserByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _userController.GetUserByIdAsync(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUser = Assert.IsType<UserDTO>(okResult.Value);
            Assert.Equal(userId, returnedUser.Id);
        }

        [Fact]
        public async Task GetUserByIdAsync_InternalServerError_Returns500()
        {
            // Arrange
            int userId = 1;
            _userServiceMock.Setup(service => service.GetUserByIdAsync(userId)).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _userController.GetUserByIdAsync(userId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", statusCodeResult.Value);
        }



    }
}