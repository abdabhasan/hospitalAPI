using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs.User;
using HospitalPresentation.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HospitalApi.Tests.Controllers.UserControllerTests
{
    public class GetUserByUsernameAsyncTests
    {
        private readonly Mock<IUser> _userServiceMock;
        private readonly UserController _userController;
        private readonly Mock<ILogger<UserController>> _loggerMock;

        public GetUserByUsernameAsyncTests()
        {
            _userServiceMock = new Mock<IUser>();
            _loggerMock = new Mock<ILogger<UserController>>();
            _userController = new UserController(_userServiceMock.Object, _loggerMock.Object);
        }



        [Fact]
        public async Task GetUserByUsernameAsync_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            string username = "nonexistentUser";
            _userServiceMock.Setup(service => service.GetUserByUsernameAsync(username)).ReturnsAsync((UserDTO)null);

            // Act
            var result = await _userController.GetUserByUsernameAsync(username);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal($"User with username {username} NOT FOUND!", notFoundResult.Value);
        }

        [Fact]
        public async Task GetUserByUsernameAsync_ValidUser_ReturnsOk()
        {
            // Arrange
            string username = "johndoe";
            var user = new UserDTO { Id = 1, Username = username };
            _userServiceMock.Setup(service => service.GetUserByUsernameAsync(username)).ReturnsAsync(user);

            // Act
            var result = await _userController.GetUserByUsernameAsync(username);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUser = Assert.IsType<UserDTO>(okResult.Value);
            Assert.Equal(username, returnedUser.Username);
        }

        [Fact]
        public async Task GetUserByUsernameAsync_InternalServerError_Returns500()
        {
            // Arrange
            string username = "johndoe";
            _userServiceMock.Setup(service => service.GetUserByUsernameAsync(username)).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _userController.GetUserByUsernameAsync(username);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", statusCodeResult.Value);
        }
    }
}