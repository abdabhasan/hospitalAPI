using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs.User;
using HospitalPresentation.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HospitalApi.Tests.Controllers.UserControllerTests
{
    public class GetAllUsersAsyncTests
    {
        private readonly Mock<IUser> _userServiceMock;
        private readonly UserController _userController;
        private readonly Mock<ILogger<UserController>> _loggerMock;

        public GetAllUsersAsyncTests()
        {
            _userServiceMock = new Mock<IUser>();
            _loggerMock = new Mock<ILogger<UserController>>();
            _userController = new UserController(_userServiceMock.Object, _loggerMock.Object);
        }


        [Fact]
        public async Task GetAllUsersAsync_NoUsersFound_ReturnsNotFound()
        {
            // Arrange
            _userServiceMock.Setup(service => service.GetAllUsersAsync()).ReturnsAsync((IEnumerable<UserDTO>)null);

            // Act
            var result = await _userController.GetAllUsersAsync();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No Users Found!", notFoundResult.Value);
        }

        [Fact]
        public async Task GetAllUsersAsync_ValidUsersList_ReturnsOk()
        {
            // Arrange
            var usersList = new List<UserDTO>
        {
            new UserDTO { Id = 1, Username = "John" },
            new UserDTO { Id = 2, Username = "Jane" }
        };
            _userServiceMock.Setup(service => service.GetAllUsersAsync()).ReturnsAsync(usersList);

            // Act
            var result = await _userController.GetAllUsersAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUsers = Assert.IsAssignableFrom<IEnumerable<UserDTO>>(okResult.Value);
            Assert.Equal(2, returnedUsers.Count());
        }

        [Fact]
        public async Task GetAllUsersAsync_InternalServerError_Returns500()
        {
            // Arrange
            _userServiceMock.Setup(service => service.GetAllUsersAsync()).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _userController.GetAllUsersAsync();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", statusCodeResult.Value);
        }

    }
}