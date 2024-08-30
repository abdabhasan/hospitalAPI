namespace HospitalDataLayer.Infrastructure.DTOs.Doctor
{
    public class DoctorDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
        public string Gender { get; set; } = "";
        public string Address { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Email { get; set; } = "";
        public DateTime BirthDate { get; set; }
        public string Specialization { get; set; } = "";
        public string? OfficeNumber { get; set; } = "";
        public int YearsOfExperience { get; set; }
        public string Qualifications { get; set; } = "";

    }
}