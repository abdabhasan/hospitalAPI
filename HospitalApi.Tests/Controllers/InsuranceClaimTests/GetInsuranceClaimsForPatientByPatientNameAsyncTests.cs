using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs;
using HospitalPresentation.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;


namespace HospitalApi.Tests.Controllers.InsuranceClaimTests
{
    public class GetInsuranceClaimsForPatientByPatientNameAsyncTests
    {
        private readonly Mock<IInsuranceClaim> _insuranceClaimServiceMock;
        private readonly InsuranceClaimController _insuranceClaimController;
        private readonly Mock<ILogger<InsuranceClaimController>> _loggerMock;

        public GetInsuranceClaimsForPatientByPatientNameAsyncTests()
        {
            _insuranceClaimServiceMock = new Mock<IInsuranceClaim>();
            _loggerMock = new Mock<ILogger<InsuranceClaimController>>();
            _insuranceClaimController = new InsuranceClaimController(_insuranceClaimServiceMock.Object, _loggerMock.Object);
        }


        [Fact]
        public async Task GetInsuranceClaimsForPatientByPatientNameAsync_ReturnsBadRequest_WhenPatientNameIsNullOrEmpty()
        {
            // Act
            var result = await _insuranceClaimController.GetInsuranceClaimsForPatientByPatientNameAsync(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid patient name.", badRequestResult.Value);
        }


        [Fact]
        public async Task GetInsuranceClaimsForPatientByPatientNameAsync_ReturnsNotFound_WhenNoInsuranceClaimsFound()
        {
            // Arrange
            _insuranceClaimServiceMock.Setup(s => s.GetInsuranceClaimsForPatientByPatientNameAsync(It.IsAny<string>()))
                                      .ReturnsAsync(Enumerable.Empty<InsuranceClaimDTO>());

            // Act
            var result = await _insuranceClaimController.GetInsuranceClaimsForPatientByPatientNameAsync("John Doe");

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No Insurance Claims Found!", notFoundResult.Value);
        }


        [Fact]
        public async Task GetInsuranceClaimsForPatientByPatientNameAsync_ReturnsOk_WhenInsuranceClaimsExist()
        {
            // Arrange
            var claims = new List<InsuranceClaimDTO>
        {
            new InsuranceClaimDTO { Id = 1,  ClaimAmount = 1000 },
            new InsuranceClaimDTO { Id = 2,  ClaimAmount = 2000 }
        };

            _insuranceClaimServiceMock.Setup(s => s.GetInsuranceClaimsForPatientByPatientNameAsync(It.IsAny<string>()))
                                      .ReturnsAsync(claims);

            // Act
            var result = await _insuranceClaimController.GetInsuranceClaimsForPatientByPatientNameAsync("John Doe");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedClaims = Assert.IsAssignableFrom<IEnumerable<InsuranceClaimDTO>>(okResult.Value);
            Assert.Equal(2, returnedClaims.Count());
        }


        [Fact]
        public async Task GetInsuranceClaimsForPatientByPatientNameAsync_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            _insuranceClaimServiceMock.Setup(s => s.GetInsuranceClaimsForPatientByPatientNameAsync(It.IsAny<string>()))
                                      .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _insuranceClaimController.GetInsuranceClaimsForPatientByPatientNameAsync("John Doe");

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, internalServerErrorResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", internalServerErrorResult.Value);
        }



    }
}