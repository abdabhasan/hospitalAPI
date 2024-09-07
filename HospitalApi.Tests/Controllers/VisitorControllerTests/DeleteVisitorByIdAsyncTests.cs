using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs.Visitor;
using HospitalPresentation.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HospitalApi.Tests.Controllers.VisitorControllerTests
{
    public class DeleteVisitorByIdAsyncTests
    {
        private readonly Mock<IVisitor> _visitorServiceMock;
        private readonly VisitorController _visitorController;
        private readonly Mock<ILogger<VisitorController>> _loggerMock;

        public DeleteVisitorByIdAsyncTests()
        {
            _visitorServiceMock = new Mock<IVisitor>();
            _loggerMock = new Mock<ILogger<VisitorController>>();
            _visitorController = new VisitorController(_visitorServiceMock.Object, _loggerMock.Object);
        }



        [Fact]
        public async Task DeleteVisitorByIdAsync_ReturnsOk_WhenVisitorIsDeleted()
        {
            // Arrange
            var visitorId = 1;
            _visitorServiceMock
                .Setup(service => service.DeleteVisitorByIdAsync(visitorId))
                .ReturnsAsync(true); // Simulate successful deletion

            // Act
            var result = await _visitorController.DeleteVisitorByIdAsync(visitorId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.True((bool)okResult.Value);
        }

        [Fact]
        public async Task DeleteVisitorByIdAsync_ReturnsNotFound_WhenVisitorDoesNotExist()
        {
            // Arrange
            var visitorId = 1;
            _visitorServiceMock
                .Setup(service => service.DeleteVisitorByIdAsync(visitorId))
                .ReturnsAsync(false); // Simulate visitor not found

            // Act
            var result = await _visitorController.DeleteVisitorByIdAsync(visitorId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Visitor not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task DeleteVisitorByIdAsync_ReturnsBadRequest_WhenInvalidVisitorId()
        {
            // Arrange
            var visitorId = -1; // Invalid visitor ID

            // Act
            var result = await _visitorController.DeleteVisitorByIdAsync(visitorId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid Visitor ID.", badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteVisitorByIdAsync_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var visitorId = 1;
            _visitorServiceMock
                .Setup(service => service.DeleteVisitorByIdAsync(visitorId))
                .ThrowsAsync(new System.Exception("Some error"));

            // Act
            var result = await _visitorController.DeleteVisitorByIdAsync(visitorId);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", objectResult.Value);
        }
    }
}