using HospitalDataLayer.Infrastructure.DTOs.Bill;

namespace HospitalBusinessLayer.Core.Interfaces
{
    public interface IBill
    {
        Task<int> CreateBillAsync(CreateBillDTO bill);
        Task<bool> DeleteBillAsyncById(int billId);
        Task<IEnumerable<BillDTO>> GetAllBillsAsync();
        Task<BillDTO> GetBillByIdAsync(int billId);
        Task<IEnumerable<BillDTO>> GetBillsForPatientByPatientIdAsync(int patientId);
        Task<IEnumerable<BillDTO>> GetBillsForPatientByPatientNameAsync(string patientName);
    }
}