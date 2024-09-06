using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs.Doctor;
using HospitalPresentation.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HospitalApi.Tests.Controllers.DoctorControllerTests
{
    public class UpdateDoctorAsyncTests
    {
        private readonly DoctorController _doctorController;
        private readonly Mock<IDoctor> _doctorServiceMock;
        private readonly Mock<ILogger<DoctorController>> _loggerMock;

        public UpdateDoctorAsyncTests()
        {
            _doctorServiceMock = new Mock<IDoctor>();
            _loggerMock = new Mock<ILogger<DoctorController>>();
            _doctorController = new DoctorController(_doctorServiceMock.Object, _loggerMock.Object);
        }
        [Fact]
        public async Task UpdateDoctorAsync_ReturnsBadRequest_WhenDoctorIdIsInvalid()
        {
            // Arrange
            var updateDoctorDto = new UpdateDoctorDTO();
            _doctorController.ModelState.AddModelError("DoctorId", "Invalid Doctor ID");

            // Act
            var result = await _doctorController.UpdateDoctorAsync(0, updateDoctorDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid Doctor ID.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateDoctorAsync_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            int doctorId = 1;
            var updateDoctorDto = new UpdateDoctorDTO();
            _doctorController.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await _doctorController.UpdateDoctorAsync(doctorId, updateDoctorDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid Doctor ID.", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateDoctorAsync_ReturnsNotFound_WhenDoctorCannotBeUpdated()
        {
            // Arrange
            int doctorId = 1;
            var updateDoctorDto = new UpdateDoctorDTO();
            _doctorServiceMock.Setup(s => s.UpdateDoctorAsync(doctorId, updateDoctorDto))
                              .ReturnsAsync(false);

            // Act
            var result = await _doctorController.UpdateDoctorAsync(doctorId, updateDoctorDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Doctor not found or could not be updated.", notFoundResult.Value);
        }

        [Fact]
        public async Task UpdateDoctorAsync_ReturnsOk_WhenDoctorIsUpdatedSuccessfully()
        {
            // Arrange
            int doctorId = 1;
            var updateDoctorDto = new UpdateDoctorDTO();
            _doctorServiceMock.Setup(s => s.UpdateDoctorAsync(doctorId, updateDoctorDto))
                              .ReturnsAsync(true);

            // Act
            var result = await _doctorController.UpdateDoctorAsync(doctorId, updateDoctorDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.True((bool)okResult.Value);
        }

        [Fact]
        public async Task UpdateDoctorAsync_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            int doctorId = 1;
            var updateDoctorDto = new UpdateDoctorDTO();
            _doctorServiceMock.Setup(s => s.UpdateDoctorAsync(doctorId, updateDoctorDto))
                              .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _doctorController.UpdateDoctorAsync(doctorId, updateDoctorDto);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, internalServerErrorResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", internalServerErrorResult.Value);
        }





    }
}