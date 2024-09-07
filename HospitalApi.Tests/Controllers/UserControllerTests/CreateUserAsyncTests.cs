
using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs.User;
using HospitalPresentation.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HospitalApi.Tests.Controllers.UserControllerTests
{
    public class CreateUserAsyncTests
    {
        private readonly Mock<IUser> _userServiceMock;
        private readonly UserController _userController;
        private readonly Mock<ILogger<UserController>> _loggerMock;

        public CreateUserAsyncTests()
        {
            _userServiceMock = new Mock<IUser>();
            _loggerMock = new Mock<ILogger<UserController>>();
            _userController = new UserController(_userServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task CreateUserAsync_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _userController.ModelState.AddModelError("error", "Invalid model state");

            // Act
            var result = await _userController.CreateUserAsync(new CreateUserDTO());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid data.", badRequestResult.Value);
        }

        [Fact]
        public async Task CreateUserAsync_ReturnsBadRequest_WhenUserCreationFails()
        {
            // Arrange
            var createUserDto = new CreateUserDTO();
            _userServiceMock.Setup(s => s.CreateUserAsync(createUserDto)).ReturnsAsync(0);

            // Act
            var result = await _userController.CreateUserAsync(createUserDto);

            // Assert
            var badRequestResult = Assert.IsType<StatusCodeResult>(result.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task CreateUserAsync_ReturnsCreated_WhenUserIsCreated()
        {
            // Arrange
            var createUserDto = new CreateUserDTO();
            _userServiceMock.Setup(s => s.CreateUserAsync(createUserDto)).ReturnsAsync(1);

            // Act
            var result = await _userController.CreateUserAsync(createUserDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtRouteResult>(result.Result);
            Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
            Assert.Equal(1, createdResult.RouteValues["UserId"]);
        }

        [Fact]
        public async Task CreateUserAsync_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var createUserDto = new CreateUserDTO();
            _userServiceMock.Setup(s => s.CreateUserAsync(createUserDto)).ThrowsAsync(new Exception("Some error"));

            // Act
            var result = await _userController.CreateUserAsync(createUserDto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", statusCodeResult.Value);
        }
    }
}