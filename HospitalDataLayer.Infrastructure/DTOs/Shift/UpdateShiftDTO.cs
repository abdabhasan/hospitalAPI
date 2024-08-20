using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalDataLayer.Infrastructure.DTOs.Shift
{
    public class UpdateShiftDTO
    {
        public int StaffId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Role { get; set; } = "";
        public string Notes { get; set; } = "";
    }
}