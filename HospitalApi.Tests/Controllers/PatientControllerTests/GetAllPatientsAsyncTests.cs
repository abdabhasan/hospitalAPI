using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs;
using HospitalPresentation.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;


namespace HospitalApi.Tests.Controllers.PatientControllerTests
{
    public class GetAllPatientsAsyncTests
    {
        private readonly Mock<IPatient> _patientServiceMock;
        private readonly PatientController _patientController;
        private readonly Mock<ILogger<PatientController>> _loggerMock;

        public GetAllPatientsAsyncTests()
        {
            _patientServiceMock = new Mock<IPatient>();
            _loggerMock = new Mock<ILogger<PatientController>>();
            _patientController = new PatientController(_patientServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetAllPatientsAsync_ReturnsNotFound_WhenNoPatientsExist()
        {
            // Arrange
            _patientServiceMock.Setup(s => s.GetAllPatientsAsync())
                               .ReturnsAsync((List<PatientDTO>)null);

            // Act
            var result = await _patientController.GetAllPatientsAsync();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No Patients Found!", notFoundResult.Value);
        }

        [Fact]
        public async Task GetAllPatientsAsync_ReturnsOk_WhenPatientsExist()
        {
            // Arrange
            var patients = new List<PatientDTO>
        {
            new PatientDTO { Id = 1, FullName = "John Doe"},
            new PatientDTO { Id = 2, FullName = "Jane Doe"}
        };
            _patientServiceMock.Setup(s => s.GetAllPatientsAsync())
                               .ReturnsAsync(patients);

            // Act
            var result = await _patientController.GetAllPatientsAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedPatients = Assert.IsType<List<PatientDTO>>(okResult.Value);
            Assert.Equal(2, returnedPatients.Count);
        }

        [Fact]
        public async Task GetAllPatientsAsync_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            _patientServiceMock.Setup(s => s.GetAllPatientsAsync())
                               .ThrowsAsync(new Exception("Some error"));

            // Act
            var result = await _patientController.GetAllPatientsAsync();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", statusCodeResult.Value);
        }

        [Fact]
        public async Task GetAllPatientsAsync_ReturnsNotFound_WhenPatientsListIsEmpty()
        {
            // Arrange
            _patientServiceMock.Setup(s => s.GetAllPatientsAsync())
                               .ReturnsAsync(new List<PatientDTO>());

            // Act
            var result = await _patientController.GetAllPatientsAsync();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No Patients Found!", notFoundResult.Value);
        }




    }
}