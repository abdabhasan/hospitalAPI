using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs.Visitor;
using HospitalPresentation.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HospitalApi.Tests.Controllers.VisitorControllerTests
{
    public class GetAllVisitorsAsyncTests
    {
        private readonly Mock<IVisitor> _visitorServiceMock;
        private readonly VisitorController _visitorController;
        private readonly Mock<ILogger<VisitorController>> _loggerMock;

        public GetAllVisitorsAsyncTests()
        {
            _visitorServiceMock = new Mock<IVisitor>();
            _loggerMock = new Mock<ILogger<VisitorController>>();
            _visitorController = new VisitorController(_visitorServiceMock.Object, _loggerMock.Object);
        }


        [Fact]
        public async Task GetAllVisitorsAsync_ReturnsOk_WhenVisitorsExist()
        {
            // Arrange
            var visitorsList = new List<VisitorDTO>
        {
            new VisitorDTO { Name = "Visitor 1" },
            new VisitorDTO { Name = "Visitor 2" }
        };
            _visitorServiceMock
                .Setup(service => service.GetAllVisitorsAsync())
                .ReturnsAsync(visitorsList);

            // Act
            var result = await _visitorController.GetAllVisitorsAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedVisitors = Assert.IsAssignableFrom<IEnumerable<VisitorDTO>>(okResult.Value);
            Assert.Equal(2, returnedVisitors.Count());
        }

        [Fact]
        public async Task GetAllVisitorsAsync_ReturnsNotFound_WhenNoVisitorsExist()
        {
            // Arrange
            _visitorServiceMock
                .Setup(service => service.GetAllVisitorsAsync())
                .ReturnsAsync((IEnumerable<VisitorDTO>)null);  // Simulate no visitors

            // Act
            var result = await _visitorController.GetAllVisitorsAsync();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No Visitors Found!", notFoundResult.Value);
        }

        [Fact]
        public async Task GetAllVisitorsAsync_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            _visitorServiceMock
                .Setup(service => service.GetAllVisitorsAsync())
                .ThrowsAsync(new System.Exception("Some error"));

            // Act
            var result = await _visitorController.GetAllVisitorsAsync();

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", objectResult.Value);
        }
    }
}