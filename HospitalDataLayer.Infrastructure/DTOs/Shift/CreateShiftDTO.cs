
namespace HospitalDataLayer.Infrastructure.DTOs.Shift
{
    public class CreateShiftDTO
    {
        public int StaffId { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Role { get; set; } = "";
        public string Notes { get; set; } = "";
    }
}