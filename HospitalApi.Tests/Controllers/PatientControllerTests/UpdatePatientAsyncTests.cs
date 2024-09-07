using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs.Patient;
using HospitalPresentation.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HospitalApi.Tests.Controllers.PatientControllerTests
{
    public class UpdatePatientAsyncTests
    {
        private readonly Mock<IPatient> _patientServiceMock;
        private readonly PatientController _patientController;
        private readonly Mock<ILogger<PatientController>> _loggerMock;

        public UpdatePatientAsyncTests()
        {
            _patientServiceMock = new Mock<IPatient>();
            _loggerMock = new Mock<ILogger<PatientController>>();
            _patientController = new PatientController(_patientServiceMock.Object, _loggerMock.Object);
        }


        [Fact]
        public async Task UpdatePatientAsync_ReturnsBadRequest_WhenPatientIdIsInvalid()
        {
            // Arrange
            int invalidPatientId = 0;
            var updatePatientDto = new UpdatePatientDTO(); // Assume valid data for simplicity

            // Act
            var result = await _patientController.UpdatePatientAsync(invalidPatientId, updatePatientDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid patient ID or data.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdatePatientAsync_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            int validPatientId = 1;
            _patientController.ModelState.AddModelError("Name", "Required"); // Simulate invalid model state
            var updatePatientDto = new UpdatePatientDTO();

            // Act
            var result = await _patientController.UpdatePatientAsync(validPatientId, updatePatientDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid patient ID or data.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdatePatientAsync_ReturnsOk_WhenPatientIsUpdated()
        {
            // Arrange
            int validPatientId = 1;
            var updatePatientDto = new UpdatePatientDTO();
            _patientServiceMock.Setup(s => s.UpdatePatientAsync(validPatientId, updatePatientDto))
                               .ReturnsAsync(true);

            // Act
            var result = await _patientController.UpdatePatientAsync(validPatientId, updatePatientDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.True((bool)okResult.Value);
        }

        [Fact]
        public async Task UpdatePatientAsync_ReturnsNotFound_WhenPatientNotFound()
        {
            // Arrange
            int validPatientId = 1;
            var updatePatientDto = new UpdatePatientDTO();
            _patientServiceMock.Setup(s => s.UpdatePatientAsync(validPatientId, updatePatientDto))
                               .ReturnsAsync(false);

            // Act
            var result = await _patientController.UpdatePatientAsync(validPatientId, updatePatientDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Patient not found or could not be updated.", notFoundResult.Value);
        }

        [Fact]
        public async Task UpdatePatientAsync_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            int validPatientId = 1;
            var updatePatientDto = new UpdatePatientDTO();
            _patientServiceMock.Setup(s => s.UpdatePatientAsync(validPatientId, updatePatientDto))
                               .ThrowsAsync(new Exception("Some error"));

            // Act
            var result = await _patientController.UpdatePatientAsync(validPatientId, updatePatientDto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", statusCodeResult.Value);
        }


    }
}