using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs;
using HospitalPresentation.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace HospitalApi.Tests.Controllers.InsuranceClaimTests
{
    public class GetAllInsuranceClaimsAsyncTests
    {
        private readonly Mock<IInsuranceClaim> _insuranceClaimServiceMock;
        private readonly InsuranceClaimController _insuranceClaimController;
        private readonly Mock<ILogger<InsuranceClaimController>> _loggerMock;

        public GetAllInsuranceClaimsAsyncTests()
        {
            _insuranceClaimServiceMock = new Mock<IInsuranceClaim>();
            _loggerMock = new Mock<ILogger<InsuranceClaimController>>();
            _insuranceClaimController = new InsuranceClaimController(_insuranceClaimServiceMock.Object, _loggerMock.Object);
        }


        [Fact]
        public async Task GetAllInsuranceClaimsAsync_ReturnsNotFound_WhenNoInsuranceClaimsFound()
        {
            // Arrange
            _insuranceClaimServiceMock.Setup(s => s.GetAllInsuranceClaimsAsync())
                                      .ReturnsAsync(Enumerable.Empty<InsuranceClaimDTO>());

            // Act
            var result = await _insuranceClaimController.GetAllInsuranceClaimsAsync();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No Insurance Claims Found!", notFoundResult.Value);
        }


        [Fact]
        public async Task GetAllInsuranceClaimsAsync_ReturnsOk_WhenInsuranceClaimsExist()
        {
            // Arrange
            var claims = new List<InsuranceClaimDTO>
        {
            new InsuranceClaimDTO { Id = 1,  ClaimAmount = 1000 },
            new InsuranceClaimDTO { Id = 2,  ClaimAmount = 2000 }
        };

            _insuranceClaimServiceMock.Setup(s => s.GetAllInsuranceClaimsAsync())
                                      .ReturnsAsync(claims);

            // Act
            var result = await _insuranceClaimController.GetAllInsuranceClaimsAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedClaims = Assert.IsAssignableFrom<IEnumerable<InsuranceClaimDTO>>(okResult.Value);
            Assert.Equal(2, returnedClaims.Count());
        }


        [Fact]
        public async Task GetAllInsuranceClaimsAsync_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            _insuranceClaimServiceMock.Setup(s => s.GetAllInsuranceClaimsAsync())
                                      .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _insuranceClaimController.GetAllInsuranceClaimsAsync();

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status500InternalServerError, internalServerErrorResult.StatusCode);
            Assert.Equal("An error occurred while processing your request.", internalServerErrorResult.Value);
        }

    }
}