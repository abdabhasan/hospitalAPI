
using HospitalDataLayer.Infrastructure.DTOs;
using HospitalDataLayer.Infrastructure.DTOs.InsuranceClaim;

namespace HospitalDataLayer.Infrastructure.Interfaces
{
    public interface IInsuranceClaimData
    {
        Task<IEnumerable<InsuranceClaimDTO>> GetAllInsuranceClaimsAsync();
        Task<IEnumerable<InsuranceClaimDTO>> GetInsuranceClaimsForPatientByPatientIdAsync(int patinetId);
        Task<IEnumerable<InsuranceClaimDTO>> GetInsuranceClaimsForPatientByPatientNameAsync(string patientName);
        Task<int> CreateInsuranceClaimAsync(CreateInsuranceClaimDTO insuranceClaim);
    }
}