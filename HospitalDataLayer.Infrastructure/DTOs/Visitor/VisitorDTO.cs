namespace HospitalDataLayer.Infrastructure.DTOs.Visitor
{
    public class VisitorDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string PatientName { get; set; } = "";
        public string? Relation { get; set; } = "";
        public DateTime VisitDate { get; set; }
        public TimeSpan VisitTime { get; set; }
        public string? Notes { get; set; } = "";

    }
}