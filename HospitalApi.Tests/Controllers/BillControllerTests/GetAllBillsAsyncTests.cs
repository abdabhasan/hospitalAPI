using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs.Bill;
using HospitalPresentation.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HospitalApi.Tests.Controllers.BillControllerTests
{
    public class GetAllBillsAsyncTests
    {


        private readonly BillController _billController;
        private readonly Mock<IBill> _billServiceMock;
        private readonly Mock<ILogger<BillController>> _loggerMock;

        public GetAllBillsAsyncTests()
        {
            _billServiceMock = new Mock<IBill>();
            _loggerMock = new Mock<ILogger<BillController>>();
            _billController = new BillController(_billServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetAllBillsAsync_ReturnsOk_WhenBillsAreFound()
        {
            // Arrange
            var bills = new List<BillDTO>
    {
        new BillDTO { Id = 1, Amount = 100, /* Other properties */ },
        new BillDTO { Id = 2, Amount = 200, /* Other properties */ }
    };

            _billServiceMock.Setup(s => s.GetAllBillsAsync())
                            .ReturnsAsync(bills); // Simulate found bills.

            // Act
            var result = await _billController.GetAllBillsAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(bills, okResult.Value);
        }


        [Fact]
        public async Task GetAllBillsAsync_ReturnsNotFound_WhenNoBillsAreFound()
        {
            // Arrange
            _billServiceMock.Setup(s => s.GetAllBillsAsync())
                            .ReturnsAsync((IEnumerable<BillDTO>)null); // Simulate no bills.

            // Act
            var result = await _billController.GetAllBillsAsync();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.Equal("No Bills Found!", notFoundResult.Value);
        }


        [Fact]
        public async Task GetAllBillsAsync_ReturnsNotFound_WhenBillsListIsEmpty()
        {
            // Arrange
            _billServiceMock.Setup(s => s.GetAllBillsAsync())
                            .ReturnsAsync(new List<BillDTO>()); // Simulate empty list.

            // Act
            var result = await _billController.GetAllBillsAsync();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.Equal("No Bills Found!", notFoundResult.Value);
        }


        [Fact]
        public async Task GetAllBillsAsync_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            _billServiceMock.Setup(s => s.GetAllBillsAsync())
                            .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _billController.GetAllBillsAsync();

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", objectResult.Value);
        }

    }
}