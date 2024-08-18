using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalDataLayer.Infrastructure.DTOs.Staff
{
    public class StaffDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
        public string Gender { get; set; } = "";
        public string Address { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Email { get; set; } = "";
        public DateTime BirthDate { get; set; }
        public string Role { get; set; } = "";
        public string Qualifications { get; set; } = "";
    }
}