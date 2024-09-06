using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs.Doctor;
using HospitalPresentation.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HospitalApi.Tests.Controllers.DoctorControllerTests
{
    public class GetDoctorOfficeNumberAsyncTests
    {
        private readonly DoctorController _doctorController;
        private readonly Mock<IDoctor> _doctorServiceMock;
        private readonly Mock<ILogger<DoctorController>> _loggerMock;

        public GetDoctorOfficeNumberAsyncTests()
        {
            _doctorServiceMock = new Mock<IDoctor>();
            _loggerMock = new Mock<ILogger<DoctorController>>();
            _doctorController = new DoctorController(_doctorServiceMock.Object, _loggerMock.Object);
        }



        [Fact]
        public async Task GetDoctorOfficeNumberAsync_ReturnsBadRequest_WhenDoctorIdIsInvalid()
        {
            // Act
            var result = await _doctorController.GetDoctorOfficeNumberAsync(0);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid doctor ID.", badRequestResult.Value);
        }


        [Fact]
        public async Task GetDoctorOfficeNumberAsync_ReturnsNotFound_WhenDoctorNotFound()
        {
            // Arrange
            int doctorId = 1;
            _doctorServiceMock.Setup(s => s.GetDoctorOfficeNumberAsync(doctorId))
                              .ReturnsAsync("Error: Doctor not found.");

            // Act
            var result = await _doctorController.GetDoctorOfficeNumberAsync(doctorId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Error: Doctor not found.", notFoundResult.Value);
        }


        [Fact]
        public async Task GetDoctorOfficeNumberAsync_ReturnsNotFound_WhenDoctorHasNoOfficeNumber()
        {
            // Arrange
            int doctorId = 1;
            _doctorServiceMock.Setup(s => s.GetDoctorOfficeNumberAsync(doctorId))
                              .ReturnsAsync("Error: Doctor has no office number.");

            // Act
            var result = await _doctorController.GetDoctorOfficeNumberAsync(doctorId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Error: Doctor has no office number.", notFoundResult.Value);
        }


        [Fact]
        public async Task GetDoctorOfficeNumberAsync_ReturnsInternalServerError_WhenUnknownErrorOccurs()
        {
            // Arrange
            int doctorId = 1;
            _doctorServiceMock.Setup(s => s.GetDoctorOfficeNumberAsync(doctorId))
                              .ReturnsAsync("Error: Some internal error.");

            // Act
            var result = await _doctorController.GetDoctorOfficeNumberAsync(doctorId);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, internalServerErrorResult.StatusCode);
            Assert.Equal("Error: Some internal error.", internalServerErrorResult.Value);
        }


        [Fact]
        public async Task GetDoctorOfficeNumberAsync_ReturnsOk_WhenDoctorHasOfficeNumber()
        {
            // Arrange
            int doctorId = 1;
            string officeNumber = "1234A";
            _doctorServiceMock.Setup(s => s.GetDoctorOfficeNumberAsync(doctorId))
                              .ReturnsAsync(officeNumber);

            // Act
            var result = await _doctorController.GetDoctorOfficeNumberAsync(doctorId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(officeNumber, okResult.Value);
        }



    }
}