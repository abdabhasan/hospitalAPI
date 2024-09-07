using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs.Visitor;
using HospitalPresentation.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HospitalApi.Tests.Controllers.VisitorControllerTests
{
    public class GetVisitorByNameAsyncTests
    {
        private readonly Mock<IVisitor> _visitorServiceMock;
        private readonly VisitorController _visitorController;
        private readonly Mock<ILogger<VisitorController>> _loggerMock;

        public GetVisitorByNameAsyncTests()
        {
            _visitorServiceMock = new Mock<IVisitor>();
            _loggerMock = new Mock<ILogger<VisitorController>>();
            _visitorController = new VisitorController(_visitorServiceMock.Object, _loggerMock.Object);
        }



        [Fact]
        public async Task GetVisitorByNameAsync_ReturnsOk_WhenVisitorsExist()
        {
            // Arrange
            var visitorName = "John Doe";
            var visitorsList = new List<VisitorDTO>
        {
            new VisitorDTO { Name = "John Doe" },
            new VisitorDTO { Name = "Jane Doe" }
        };
            _visitorServiceMock
                .Setup(service => service.GetVisitorByNameAsync(visitorName))
                .ReturnsAsync(visitorsList);

            // Act
            var result = await _visitorController.GetVisitorByNameAsync(visitorName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedVisitors = Assert.IsAssignableFrom<IEnumerable<VisitorDTO>>(okResult.Value);
            Assert.Equal(2, returnedVisitors.Count());
        }

        [Fact]
        public async Task GetVisitorByNameAsync_ReturnsNotFound_WhenNoVisitorsExist()
        {
            // Arrange
            var visitorName = "Unknown";
            _visitorServiceMock
                .Setup(service => service.GetVisitorByNameAsync(visitorName))
                .ReturnsAsync((IEnumerable<VisitorDTO>)null);  // Simulate no visitors found

            // Act
            var result = await _visitorController.GetVisitorByNameAsync(visitorName);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No Visitors Found!", notFoundResult.Value);
        }

        [Fact]
        public async Task GetVisitorByNameAsync_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var visitorName = "John Doe";
            _visitorServiceMock
                .Setup(service => service.GetVisitorByNameAsync(visitorName))
                .ThrowsAsync(new System.Exception("Some error"));

            // Act
            var result = await _visitorController.GetVisitorByNameAsync(visitorName);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", objectResult.Value);
        }
    }
}