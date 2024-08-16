namespace HospitalDataLayer.Infrastructure.DTOs
{
    public class PatientDTO
    {

        public int Id { get; set; }
        public string FullName { get; set; } = "";
        public string Gender { get; set; } = "";
        public string Address { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Email { get; set; } = "";
        public DateTime BirthDate { get; set; }
        public string EmergencyContactName { get; set; } = "";
        public string EmergencyContactPhone { get; set; } = "";
        public string InsuranceProvider { get; set; } = "";
        public string InsurancePolicyNumber { get; set; } = "";
        public string MedicalHistory { get; set; } = "";
        public string Allergies { get; set; } = "";

    }
}