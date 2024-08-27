
using HospitalDataLayer.Infrastructure.DTOs;

namespace HospitalDataLayer.Infrastructure.Interfaces
{
    public interface IInsuranceClaimData
    {
        Task<IEnumerable<InsuranceClaimDTO>> GetAllInsuranceClaimsAsync();
        Task<IEnumerable<InsuranceClaimDTO>> GetInsuranceClaimsForPatientByPatientIdAsync(int patinetId);
        Task<IEnumerable<InsuranceClaimDTO>> GetInsuranceClaimsForPatientByPatientNameAsync(string patientName);
    }
}