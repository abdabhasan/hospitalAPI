using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs;
using HospitalPresentation.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HospitalApi.Tests.Controllers.PatientControllerTests
{
    public class GetPatientByIdAsyncTests
    {
        private readonly Mock<IPatient> _patientServiceMock;
        private readonly PatientController _patientController;
        private readonly Mock<ILogger<PatientController>> _loggerMock;

        public GetPatientByIdAsyncTests()
        {
            _patientServiceMock = new Mock<IPatient>();
            _loggerMock = new Mock<ILogger<PatientController>>();
            _patientController = new PatientController(_patientServiceMock.Object, _loggerMock.Object);
        }




        [Fact]
        public async Task GetPatientByIdAsync_ReturnsNotFound_WhenPatientDoesNotExist()
        {
            // Arrange
            int patientId = 1;
            _patientServiceMock.Setup(s => s.GetPatientByIdAsync(patientId))
                               .ReturnsAsync((PatientDTO)null);

            // Act
            var result = await _patientController.GetPatientByIdAsync(patientId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal($"Patient with Id {patientId} NOT FOUND!", notFoundResult.Value);
        }

        [Fact]
        public async Task GetPatientByIdAsync_ReturnsOk_WhenPatientExists()
        {
            // Arrange
            int patientId = 1;
            var patient = new PatientDTO { Id = patientId, FullName = "John Doe" };
            _patientServiceMock.Setup(s => s.GetPatientByIdAsync(patientId))
                               .ReturnsAsync(patient);

            // Act
            var result = await _patientController.GetPatientByIdAsync(patientId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedPatient = Assert.IsType<PatientDTO>(okResult.Value);
            Assert.Equal(patientId, returnedPatient.Id);
        }

        [Fact]
        public async Task GetPatientByIdAsync_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            int patientId = 1;
            _patientServiceMock.Setup(s => s.GetPatientByIdAsync(patientId))
                               .ThrowsAsync(new Exception("Some error"));

            // Act
            var result = await _patientController.GetPatientByIdAsync(patientId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", statusCodeResult.Value);
        }

        [Fact]
        public async Task GetPatientByIdAsync_ReturnsBadRequest_WhenInvalidPatientIdIsProvided()
        {
            // Arrange
            int invalidPatientId = -1;

            // Act
            var result = await _patientController.GetPatientByIdAsync(invalidPatientId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid patient ID.", badRequestResult.Value);
        }
    }
}