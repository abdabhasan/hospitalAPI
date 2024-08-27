
using HospitalDataLayer.Infrastructure.DTOs;

namespace HospitalDataLayer.Infrastructure.Interfaces
{
    public interface IInsuranceClaimData
    {
        Task<IEnumerable<InsuranceClaimDTO>> GetAllInsuranceClaimsAsync();
        Task<InsuranceClaimDTO> GetInsuranceClaimsForPatientByPatientIdAsync(int patinetId);
        Task<InsuranceClaimDTO> GetInsuranceClaimsForPatientByPatientNameAsync(string patinetName);
    }
}