using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs.Doctor;
using HospitalPresentation.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HospitalApi.Tests.Controllers.DoctorControllerTests
{
    public class CreateDoctorAsyncTests
    {
        private readonly DoctorController _doctorController;
        private readonly Mock<IDoctor> _doctorServiceMock;
        private readonly Mock<ILogger<DoctorController>> _loggerMock;

        public CreateDoctorAsyncTests()
        {
            _doctorServiceMock = new Mock<IDoctor>();
            _loggerMock = new Mock<ILogger<DoctorController>>();
            _doctorController = new DoctorController(_doctorServiceMock.Object, _loggerMock.Object);
        }



        [Fact]
        public async Task CreateDoctorAsync_ReturnsCreated_WhenDoctorIsSuccessfullyCreated()
        {
            // Arrange
            var createDoctorDto = new CreateDoctorDTO
            {
                FirstName = "Dr. Smith",
                LastName = "Doe",
                Specialization = "Cardiology"
            };
            var createdDoctorId = 1;

            _doctorServiceMock.Setup(s => s.CreateDoctorAsync(createDoctorDto))
                              .ReturnsAsync(createdDoctorId);

            // Act
            var result = await _doctorController.CreateDoctorAsync(createDoctorDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtRouteResult>(result.Result);
            Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
            Assert.Equal(createdDoctorId, createdResult.Value);
            Assert.Equal("GetDoctorById", createdResult.RouteName);
        }


        [Fact]
        public async Task CreateDoctorAsync_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _doctorController.ModelState.AddModelError("FirstName", "The FirstName field is required.");

            var createDoctorDto = new CreateDoctorDTO(); // Invalid DTO due to missing required fields

            // Act
            var result = await _doctorController.CreateDoctorAsync(createDoctorDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.Equal("Invalid data.", badRequestResult.Value);
        }


        [Fact]
        public async Task CreateDoctorAsync_ReturnsBadRequest_WhenDoctorCreationFails()
        {
            // Arrange
            var createDoctorDto = new CreateDoctorDTO
            {
                FirstName = "Dr. Smith",
                LastName = "Doe",
                Specialization = "Cardiology"
            };

            var failedDoctorId = 0;

            _doctorServiceMock.Setup(s => s.CreateDoctorAsync(createDoctorDto))
                              .ReturnsAsync(failedDoctorId);

            // Act
            var result = await _doctorController.CreateDoctorAsync(createDoctorDto);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, statusCodeResult.StatusCode);
        }


        [Fact]
        public async Task CreateDoctorAsync_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var createDoctorDto = new CreateDoctorDTO
            {
                FirstName = "Dr. Smith",
                LastName = "Doe",
                Specialization = "Cardiology"
            };

            _doctorServiceMock.Setup(s => s.CreateDoctorAsync(createDoctorDto))
                              .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _doctorController.CreateDoctorAsync(createDoctorDto);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", objectResult.Value);
        }




    }
}