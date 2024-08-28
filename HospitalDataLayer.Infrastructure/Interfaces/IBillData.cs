using HospitalDataLayer.Infrastructure.DTOs.Bill;

namespace HospitalDataLayer.Infrastructure.Interfaces
{
    public interface IBillData
    {
        Task<IEnumerable<BillDTO>> GetAllBillsAsync();
        Task<IEnumerable<BillDTO>> GetBillsForPatientByPatientIdAsync(int patientId);
        Task<IEnumerable<BillDTO>> GetBillsForPatientByPatientNameAsync(string patientName);
        Task<BillDTO> GetBillByIdAsync(int billId);
        Task<int> CreateBillAsync(CreateBillDTO bill);
        Task<bool> DeleteBillAsync(int billId);
    }
}