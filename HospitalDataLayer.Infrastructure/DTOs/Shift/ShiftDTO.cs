
namespace HospitalDataLayer.Infrastructure.DTOs.Shift
{
    public class ShiftDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Role { get; set; } = "";
        public string Notes { get; set; } = "";

    }
}