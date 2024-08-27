
using HospitalDataLayer.Infrastructure.DTOs;
using HospitalDataLayer.Infrastructure.Interfaces;

namespace HospitalBusinessLayer.Core
{
    public class clsInsuranceClaim
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


    }
}