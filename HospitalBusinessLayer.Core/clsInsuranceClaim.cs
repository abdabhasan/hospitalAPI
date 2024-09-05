
using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs;
using HospitalDataLayer.Infrastructure.DTOs.InsuranceClaim;
using HospitalDataLayer.Infrastructure.Interfaces;

namespace HospitalBusinessLayer.Core
{
    public class clsInsuranceClaim : IInsuranceClaim
    {
        private readonly IInsuranceClaimData _insuranceClaim;
        public clsInsuranceClaim(IInsuranceClaimData insuranceClaim)
        {
            _insuranceClaim = insuranceClaim;
        }


        public async Task<IEnumerable<InsuranceClaimDTO>> GetAllInsuranceClaimsAsync()
        {
            return await _insuranceClaim.GetAllInsuranceClaimsAsync();
        }

        public async Task<IEnumerable<InsuranceClaimDTO>> GetInsuranceClaimsForPatientByPatientNameAsync(string patientName)
        {
            return await _insuranceClaim.GetInsuranceClaimsForPatientByPatientNameAsync(patientName);
        }

        public async Task<IEnumerable<InsuranceClaimDTO>> GetInsuranceClaimsForPatientByPatientIdAsync(int patientId)
        {
            return await _insuranceClaim.GetInsuranceClaimsForPatientByPatientIdAsync(patientId);
        }

        public async Task<int> CreateInsuranceClaimAsync(CreateInsuranceClaimDTO insuranceClaim)
        {
            return await _insuranceClaim.CreateInsuranceClaimAsync(insuranceClaim);
        }

        public async Task<bool> DeleteInsuranceClaimAsync(int claimId)
        {
            return await _insuranceClaim.DeleteInsuranceClaimAsync(claimId);
        }


    }
}