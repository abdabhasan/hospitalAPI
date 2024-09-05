using HospitalDataLayer.Infrastructure.DTOs;
using HospitalDataLayer.Infrastructure.DTOs.InsuranceClaim;

namespace HospitalBusinessLayer.Core.Interfaces
{
    public interface IInsuranceClaim
    {
        Task<IEnumerable<InsuranceClaimDTO>> GetAllInsuranceClaimsAsync();
        Task<IEnumerable<InsuranceClaimDTO>> GetInsuranceClaimsForPatientByPatientNameAsync(string patientName);
        Task<IEnumerable<InsuranceClaimDTO>> GetInsuranceClaimsForPatientByPatientIdAsync(int patientId);
        Task<int> CreateInsuranceClaimAsync(CreateInsuranceClaimDTO insuranceClaim);
        Task<bool> DeleteInsuranceClaimAsync(int claimId);
    }
}