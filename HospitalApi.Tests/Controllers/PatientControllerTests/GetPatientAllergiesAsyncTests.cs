using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs;
using HospitalDataLayer.Infrastructure.DTOs.Patient;
using HospitalPresentation.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HospitalApi.Tests.Controllers.PatientControllerTests
{
    public class GetPatientAllergiesAsyncTests
    {
        private readonly Mock<IPatient> _patientServiceMock;
        private readonly PatientController _patientController;
        private readonly Mock<ILogger<PatientController>> _loggerMock;

        public GetPatientAllergiesAsyncTests()
        {
            _patientServiceMock = new Mock<IPatient>();
            _loggerMock = new Mock<ILogger<PatientController>>();
            _patientController = new PatientController(_patientServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetPatientAllergiesAsync_ReturnsBadRequest_WhenPatientIdIsInvalid()
        {
            // Arrange
            int invalidPatientId = 0;

            // Act
            var result = await _patientController.GetPatientAllergiesAsync(invalidPatientId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid patient ID.", badRequestResult.Value);
        }

        [Fact]
        public async Task GetPatientAllergiesAsync_ReturnsNotFound_WhenPatientNotFound()
        {
            // Arrange
            int validPatientId = 1;
            _patientServiceMock.Setup(s => s.GetPatientAllergiesAsync(validPatientId))
                               .ReturnsAsync("Error: Patient not found.");

            // Act
            var result = await _patientController.GetPatientAllergiesAsync(validPatientId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Error: Patient not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetPatientAllergiesAsync_ReturnsNotFound_WhenNoAllergiesFound()
        {
            // Arrange
            int validPatientId = 1;
            _patientServiceMock.Setup(s => s.GetPatientAllergiesAsync(validPatientId))
                               .ReturnsAsync("Error: Patient has no allergies.");

            // Act
            var result = await _patientController.GetPatientAllergiesAsync(validPatientId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Error: Patient has no allergies.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetPatientAllergiesAsync_ReturnsInternalServerError_WhenErrorStartsWithErrorPrefix()
        {
            // Arrange
            int validPatientId = 1;
            _patientServiceMock.Setup(s => s.GetPatientAllergiesAsync(validPatientId))
                               .ReturnsAsync("Error: Some internal error");

            // Act
            var result = await _patientController.GetPatientAllergiesAsync(validPatientId);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, internalServerErrorResult.StatusCode);
            Assert.Equal("Error: Some internal error", internalServerErrorResult.Value);
        }

        [Fact]
        public async Task GetPatientAllergiesAsync_ReturnsOk_WhenAllergiesAreAvailable()
        {
            // Arrange
            int validPatientId = 1;
            string expectedAllergies = "Patient is allergic to peanuts.";
            _patientServiceMock.Setup(s => s.GetPatientAllergiesAsync(validPatientId))
                               .ReturnsAsync(expectedAllergies);

            // Act
            var result = await _patientController.GetPatientAllergiesAsync(validPatientId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(expectedAllergies, okResult.Value);
        }

        [Fact]
        public async Task GetPatientAllergiesAsync_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            int validPatientId = 1;
            _patientServiceMock.Setup(s => s.GetPatientAllergiesAsync(validPatientId))
                               .ThrowsAsync(new Exception("Some error"));

            // Act
            var result = await _patientController.GetPatientAllergiesAsync(validPatientId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", statusCodeResult.Value);
        }


    }
}