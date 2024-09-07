using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs.Shift;
using HospitalPresentation.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HospitalApi.Tests.Controllers.ShiftControllerTests
{
    public class GetAllShiftsAsyncTests
    {
        private readonly Mock<IShift> _shiftServiceMock;
        private readonly ShiftController _shiftController;
        private readonly Mock<ILogger<ShiftController>> _loggerMock;

        public GetAllShiftsAsyncTests()
        {
            _shiftServiceMock = new Mock<IShift>();
            _loggerMock = new Mock<ILogger<ShiftController>>();
            _shiftController = new ShiftController(_shiftServiceMock.Object, _loggerMock.Object);
        }
        [Fact]
        public async Task GetAllShiftsAsync_ReturnsNotFound_WhenNoShiftsFound()
        {
            // Arrange
            _shiftServiceMock.Setup(s => s.GetAllShiftsAsync())
                             .ReturnsAsync(new List<ShiftDTO>());

            // Act
            var result = await _shiftController.GetAllShiftsAsync();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No Shifts Found!", notFoundResult.Value);
        }

        [Fact]
        public async Task GetAllShiftsAsync_ReturnsOk_WhenShiftsAreFound()
        {
            // Arrange
            var shifts = new List<ShiftDTO>
        {
            new ShiftDTO { Id = 1,  Date = DateTime.Now  },
            new ShiftDTO { Id = 2,  Date = DateTime.Now }
        };
            _shiftServiceMock.Setup(s => s.GetAllShiftsAsync())
                             .ReturnsAsync(shifts);

            // Act
            var result = await _shiftController.GetAllShiftsAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(shifts, okResult.Value);
        }

        [Fact]
        public async Task GetAllShiftsAsync_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            _shiftServiceMock.Setup(s => s.GetAllShiftsAsync())
                             .ThrowsAsync(new Exception("Some error"));

            // Act
            var result = await _shiftController.GetAllShiftsAsync();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", statusCodeResult.Value);
        }


    }
}