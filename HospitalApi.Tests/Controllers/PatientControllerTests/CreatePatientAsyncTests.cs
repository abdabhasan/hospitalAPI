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
    public class CreatePatientAsyncTests
    {
        private readonly Mock<IPatient> _patientServiceMock;
        private readonly PatientController _patientController;
        private readonly Mock<ILogger<PatientController>> _loggerMock;

        public CreatePatientAsyncTests()
        {
            _patientServiceMock = new Mock<IPatient>();
            _loggerMock = new Mock<ILogger<PatientController>>();
            _patientController = new PatientController(_patientServiceMock.Object, _loggerMock.Object);
        }


        [Fact]
        public async Task CreatePatientAsync_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _patientController.ModelState.AddModelError("Error", "Invalid data");
            var createPatientDto = new CreatePatientDTO();

            // Act
            var result = await _patientController.CreatePatientAsync(createPatientDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid data.", badRequestResult.Value);
        }

        [Fact]
        public async Task CreatePatientAsync_ReturnsCreatedAtRoute_WhenPatientIsCreated()
        {
            // Arrange
            var createPatientDto = new CreatePatientDTO { FirstName = "John", LastName = "Doe" };
            int expectedPatientId = 1;
            _patientServiceMock.Setup(s => s.CreatePatientAsync(createPatientDto))
                               .ReturnsAsync(expectedPatientId);

            // Act
            var result = await _patientController.CreatePatientAsync(createPatientDto);

            // Assert
            var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result.Result);
            Assert.Equal("GetPatientById", createdAtRouteResult.RouteName);
            Assert.Equal(expectedPatientId, createdAtRouteResult.RouteValues["PatientId"]);
            Assert.Equal(expectedPatientId, createdAtRouteResult.Value);
        }

        [Fact]
        public async Task CreatePatientAsync_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var createPatientDto = new CreatePatientDTO { FirstName = "John", LastName = "Doe" };
            _patientServiceMock.Setup(s => s.CreatePatientAsync(createPatientDto))
                               .ThrowsAsync(new Exception("Some error"));

            // Act
            var result = await _patientController.CreatePatientAsync(createPatientDto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", statusCodeResult.Value);
        }



    }
}