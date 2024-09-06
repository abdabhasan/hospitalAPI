using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs;
using HospitalPresentation.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HospitalApi.Tests.Controllers.InsuranceClaimTests
{
    public class DeleteInsuranceClaimAsyncTests
    {
        private readonly Mock<IInsuranceClaim> _insuranceClaimServiceMock;
        private readonly InsuranceClaimController _insuranceClaimController;
        private readonly Mock<ILogger<InsuranceClaimController>> _loggerMock;

        public DeleteInsuranceClaimAsyncTests()
        {
            _insuranceClaimServiceMock = new Mock<IInsuranceClaim>();
            _loggerMock = new Mock<ILogger<InsuranceClaimController>>();
            _insuranceClaimController = new InsuranceClaimController(_insuranceClaimServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task DeleteInsuranceClaimAsync_ReturnsBadRequest_WhenInsuranceClaimIdIsInvalid()
        {
            // Act
            var result = await _insuranceClaimController.DeleteInsuranceClaimAsync(0);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid InsuranceClaim ID.", badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteInsuranceClaimAsync_ReturnsNotFound_WhenInsuranceClaimDoesNotExist()
        {
            // Arrange
            _insuranceClaimServiceMock.Setup(s => s.DeleteInsuranceClaimAsync(It.IsAny<int>()))
                                      .ReturnsAsync(false);

            // Act
            var result = await _insuranceClaimController.DeleteInsuranceClaimAsync(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("InsuranceClaim not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task DeleteInsuranceClaimAsync_ReturnsOk_WhenInsuranceClaimIsDeletedSuccessfully()
        {
            // Arrange
            _insuranceClaimServiceMock.Setup(s => s.DeleteInsuranceClaimAsync(It.IsAny<int>()))
                                      .ReturnsAsync(true);

            // Act
            var result = await _insuranceClaimController.DeleteInsuranceClaimAsync(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.True((bool)okResult.Value);
        }

        [Fact]
        public async Task DeleteInsuranceClaimAsync_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            _insuranceClaimServiceMock.Setup(s => s.DeleteInsuranceClaimAsync(It.IsAny<int>()))
                                      .ThrowsAsync(new Exception("Some error"));

            // Act
            var result = await _insuranceClaimController.DeleteInsuranceClaimAsync(1);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", statusCodeResult.Value);
        }



    }
}