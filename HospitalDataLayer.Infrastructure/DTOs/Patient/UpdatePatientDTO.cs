using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalDataLayer.Infrastructure.DTOs.Patient
{
    public class UpdatePatientDTO
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
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