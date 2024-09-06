using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs.Doctor;
using HospitalPresentation.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;


namespace HospitalApi.Tests.Controllers.DoctorControllerTests
{
    public class GetDoctorByIdAsyncTests
    {
        private readonly DoctorController _doctorController;
        private readonly Mock<IDoctor> _doctorServiceMock;
        private readonly Mock<ILogger<DoctorController>> _loggerMock;

        public GetDoctorByIdAsyncTests()
        {
            _doctorServiceMock = new Mock<IDoctor>();
            _loggerMock = new Mock<ILogger<DoctorController>>();
            _doctorController = new DoctorController(_doctorServiceMock.Object, _loggerMock.Object);
        }


        [Fact]
        public async Task GetDoctorByIdAsync_ReturnsOk_WhenDoctorIsFound()
        {
            // Arrange
            var doctorId = 1;
            var doctor = new DoctorDTO { Id = doctorId, FullName = "Dr. Smith", Specialization = "Cardiology" };

            _doctorServiceMock.Setup(s => s.GetDoctorByIdAsync(doctorId))
                              .ReturnsAsync(doctor);

            // Act
            var result = await _doctorController.GetDoctorByIdAsync(doctorId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(doctor, okResult.Value);
        }


        [Fact]
        public async Task GetDoctorByIdAsync_ReturnsNotFound_WhenDoctorIsNotFound()
        {
            // Arrange
            var doctorId = 1;

            _doctorServiceMock.Setup(s => s.GetDoctorByIdAsync(doctorId))
                              .ReturnsAsync((DoctorDTO)null);

            // Act
            var result = await _doctorController.GetDoctorByIdAsync(doctorId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.Equal($"Doctor with Id {doctorId} NOT FOUND!", notFoundResult.Value);
        }


        [Fact]
        public async Task GetDoctorByIdAsync_ReturnsBadRequest_WhenDoctorIdIsInvalid()
        {
            // Arrange
            var invalidDoctorId = -1;

            // Act
            var result = await _doctorController.GetDoctorByIdAsync(invalidDoctorId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.Equal("Invalid doctor ID.", badRequestResult.Value);
        }


        [Fact]
        public async Task GetDoctorByIdAsync_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var doctorId = 1;

            _doctorServiceMock.Setup(s => s.GetDoctorByIdAsync(doctorId))
                              .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _doctorController.GetDoctorByIdAsync(doctorId);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", objectResult.Value);
        }


    }
}