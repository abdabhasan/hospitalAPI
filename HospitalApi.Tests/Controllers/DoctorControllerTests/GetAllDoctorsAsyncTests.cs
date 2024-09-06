using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs.Doctor;
using HospitalPresentation.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HospitalApi.Tests.Controllers.DoctorControllerTests
{
    public class GetAllDoctorsAsyncTests
    {

        private readonly DoctorController _doctorController;
        private readonly Mock<IDoctor> _doctorServiceMock;
        private readonly Mock<ILogger<DoctorController>> _loggerMock;

        public GetAllDoctorsAsyncTests()
        {
            _doctorServiceMock = new Mock<IDoctor>();
            _loggerMock = new Mock<ILogger<DoctorController>>();
            _doctorController = new DoctorController(_doctorServiceMock.Object, _loggerMock.Object);
        }


        [Fact]
        public async Task GetAllDoctorsAsync_ReturnsOk_WhenDoctorsAreFound()
        {
            // Arrange
            var doctors = new List<DoctorDTO>
    {
        new DoctorDTO { Id = 1, FullName = "Dr. Smith", Specialization = "Cardiology" },
        new DoctorDTO { Id = 2, FullName = "Dr. Johnson", Specialization = "Neurology" }
    };

            _doctorServiceMock.Setup(s => s.GetAllDoctorsAsync())
                              .ReturnsAsync(doctors);

            // Act
            var result = await _doctorController.GetAllDoctorsAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(doctors, okResult.Value);
        }


        [Fact]
        public async Task GetAllDoctorsAsync_ReturnsNotFound_WhenNoDoctorsAreFound()
        {
            // Arrange
            _doctorServiceMock.Setup(s => s.GetAllDoctorsAsync())
                              .ReturnsAsync((List<DoctorDTO>)null); // Simulate no doctors.

            // Act
            var result = await _doctorController.GetAllDoctorsAsync();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.Equal("No Doctors Found!", notFoundResult.Value);
        }


        [Fact]
        public async Task GetAllDoctorsAsync_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            _doctorServiceMock.Setup(s => s.GetAllDoctorsAsync())
                              .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _doctorController.GetAllDoctorsAsync();

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", objectResult.Value);
        }

    }
}