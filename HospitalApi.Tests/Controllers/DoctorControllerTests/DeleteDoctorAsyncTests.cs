using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs.Doctor;
using HospitalPresentation.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HospitalApi.Tests.Controllers.DoctorControllerTests
{
    public class DeleteDoctorAsyncTests
    {
        private readonly DoctorController _doctorController;
        private readonly Mock<IDoctor> _doctorServiceMock;
        private readonly Mock<ILogger<DoctorController>> _loggerMock;

        public DeleteDoctorAsyncTests()
        {
            _doctorServiceMock = new Mock<IDoctor>();
            _loggerMock = new Mock<ILogger<DoctorController>>();
            _doctorController = new DoctorController(_doctorServiceMock.Object, _loggerMock.Object);
        }


        [Fact]
        public async Task DeleteDoctorAsync_ReturnsBadRequest_WhenDoctorIdIsInvalid()
        {
            // Act
            var result = await _doctorController.DeleteDoctorAsync(0);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid Doctor ID.", badRequestResult.Value);
        }


        [Fact]
        public async Task DeleteDoctorAsync_ReturnsNotFound_WhenDoctorCannotBeFound()
        {
            // Arrange
            int doctorId = 1;
            _doctorServiceMock.Setup(s => s.DeleteDoctorAsync(doctorId))
                              .ReturnsAsync(false);

            // Act
            var result = await _doctorController.DeleteDoctorAsync(doctorId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Doctor not found.", notFoundResult.Value);
        }


        [Fact]
        public async Task DeleteDoctorAsync_ReturnsOk_WhenDoctorIsDeletedSuccessfully()
        {
            // Arrange
            int doctorId = 1;
            _doctorServiceMock.Setup(s => s.DeleteDoctorAsync(doctorId))
                              .ReturnsAsync(true);

            // Act
            var result = await _doctorController.DeleteDoctorAsync(doctorId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.True((bool)okResult.Value);
        }


        [Fact]
        public async Task DeleteDoctorAsync_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            int doctorId = 1;
            _doctorServiceMock.Setup(s => s.DeleteDoctorAsync(doctorId))
                              .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _doctorController.DeleteDoctorAsync(doctorId);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, internalServerErrorResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", internalServerErrorResult.Value);
        }

    }
}