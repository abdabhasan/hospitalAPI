
namespace HospitalDataLayer.Infrastructure.DTOs.Bill
{
    public class BillDTO
    {
        public int Id { get; set; }
        public int PatinetId { get; set; }
        public int Amount { get; set; }
        public DateTime DateIssued { get; set; }
        public DateTime? DueDate { get; set; }
        public string PaymentStatus { get; set; } = "NOT PAIED";
        public string Notes { get; set; } = "";
        public string PaymentMethod { get; set; } = "CASH";

    }
}