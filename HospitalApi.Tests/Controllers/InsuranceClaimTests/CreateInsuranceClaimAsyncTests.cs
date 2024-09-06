using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs.InsuranceClaim;
using HospitalPresentation.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HospitalApi.Tests.Controllers.InsuranceClaimTests
{
    public class CreateInsuranceClaimAsyncTests
    {
        private readonly Mock<IInsuranceClaim> _insuranceClaimServiceMock;
        private readonly InsuranceClaimController _insuranceClaimController;
        private readonly Mock<ILogger<InsuranceClaimController>> _loggerMock;

        public CreateInsuranceClaimAsyncTests()
        {
            _insuranceClaimServiceMock = new Mock<IInsuranceClaim>();
            _loggerMock = new Mock<ILogger<InsuranceClaimController>>();
            _insuranceClaimController = new InsuranceClaimController(_insuranceClaimServiceMock.Object, _loggerMock.Object);
        }





        [Fact]
        public async Task CreateInsuranceClaimAsync_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _insuranceClaimController.ModelState.AddModelError("Error", "Invalid model");

            // Act
            var result = await _insuranceClaimController.CreateInsuranceClaimAsync(new CreateInsuranceClaimDTO());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid data.", badRequestResult.Value);
        }

        [Fact]
        public async Task CreateInsuranceClaimAsync_ReturnsBadRequest_WhenInsuranceClaimCreationFails()
        {
            // Arrange
            _insuranceClaimServiceMock.Setup(s => s.CreateInsuranceClaimAsync(It.IsAny<CreateInsuranceClaimDTO>()))
                                      .ReturnsAsync(0);

            // Act
            var result = await _insuranceClaimController.CreateInsuranceClaimAsync(new CreateInsuranceClaimDTO());


            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task CreateInsuranceClaimAsync_ReturnsCreatedAtRoute_WhenInsuranceClaimIsCreated()
        {
            // Arrange
            var insuranceClaimId = 1;
            _insuranceClaimServiceMock.Setup(s => s.CreateInsuranceClaimAsync(It.IsAny<CreateInsuranceClaimDTO>()))
                                      .ReturnsAsync(insuranceClaimId);

            // Act
            var result = await _insuranceClaimController.CreateInsuranceClaimAsync(new CreateInsuranceClaimDTO());

            // Assert
            var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result.Result);
            Assert.Equal("GetInsuranceClaimsForPatientByPatientId", createdAtRouteResult.RouteName);
            Assert.Equal(insuranceClaimId, createdAtRouteResult.RouteValues["InsuranceClaimId"]);
            Assert.Equal(insuranceClaimId, createdAtRouteResult.Value);
        }

        [Fact]
        public async Task CreateInsuranceClaimAsync_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            _insuranceClaimServiceMock.Setup(s => s.CreateInsuranceClaimAsync(It.IsAny<CreateInsuranceClaimDTO>()))
                                      .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _insuranceClaimController.CreateInsuranceClaimAsync(new CreateInsuranceClaimDTO());

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, internalServerErrorResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", internalServerErrorResult.Value);
        }



    }
}