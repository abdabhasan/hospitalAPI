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
    public class DeletePatientAsyncTests
    {
        private readonly Mock<IPatient> _patientServiceMock;
        private readonly PatientController _patientController;
        private readonly Mock<ILogger<PatientController>> _loggerMock;

        public DeletePatientAsyncTests()
        {
            _patientServiceMock = new Mock<IPatient>();
            _loggerMock = new Mock<ILogger<PatientController>>();
            _patientController = new PatientController(_patientServiceMock.Object, _loggerMock.Object);
        }


        [Fact]
        public async Task DeletePatientAsync_ReturnsBadRequest_WhenPatientIdIsInvalid()
        {
            // Arrange
            int invalidPatientId = 0;

            // Act
            var result = await _patientController.DeletePatientAsync(invalidPatientId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid patient ID.", badRequestResult.Value);
        }

        [Fact]
        public async Task DeletePatientAsync_ReturnsOk_WhenPatientIsDeleted()
        {
            // Arrange
            int validPatientId = 1;
            _patientServiceMock.Setup(s => s.DeletePatientAsync(validPatientId))
                               .ReturnsAsync(true);

            // Act
            var result = await _patientController.DeletePatientAsync(validPatientId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.True((bool)okResult.Value);
        }

        [Fact]
        public async Task DeletePatientAsync_ReturnsNotFound_WhenPatientNotFound()
        {
            // Arrange
            int validPatientId = 1;
            _patientServiceMock.Setup(s => s.DeletePatientAsync(validPatientId))
                               .ReturnsAsync(false);

            // Act
            var result = await _patientController.DeletePatientAsync(validPatientId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Patient not found or could not be deleted.", notFoundResult.Value);
        }

        [Fact]
        public async Task DeletePatientAsync_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            int validPatientId = 1;
            _patientServiceMock.Setup(s => s.DeletePatientAsync(validPatientId))
                               .ThrowsAsync(new Exception("Some error"));

            // Act
            var result = await _patientController.DeletePatientAsync(validPatientId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", statusCodeResult.Value);
        }


    }
}