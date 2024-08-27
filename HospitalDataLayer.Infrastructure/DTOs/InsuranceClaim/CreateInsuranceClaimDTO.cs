
namespace HospitalDataLayer.Infrastructure.DTOs.InsuranceClaim
{
    public class CreateInsuranceClaimDTO
    {
        public int PatientId { get; set; }
        public string InsuranceProvider { get; set; } = "";
        public string? PolicyNumber { get; set; } = "";
        public DateTime ClaimDate { get; set; }
        public int ClaimAmount { get; set; }
        public string ClaimStatus { get; set; } = "";
        public string? Notes { get; set; } = "";
    }
}