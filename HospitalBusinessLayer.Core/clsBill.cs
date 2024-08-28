using HospitalDataLayer.Infrastructure.DTOs.Bill;
using HospitalDataLayer.Infrastructure.Interfaces;

namespace HospitalBusinessLayer.Core
{
    public class clsBill
    {
        private readonly IBillData _billData;
        public clsBill(IBillData billData)
        {
            _billData = billData;
        }


        public async Task<int> CreateBillAsync(CreateBillDTO bill)
        {
            return await _billData.CreateBillAsync(bill);
        }

        public async Task<bool> DeleteBillAsyncById(int billId)
        {
            return await _billData.DeleteBillAsyncById(billId);
        }

        public async Task<IEnumerable<BillDTO>> GetAllBillsAsync()
        {
            return await _billData.GetAllBillsAsync();
        }

        public async Task<BillDTO> GetBillByIdAsync(int billId)
        {
            return await _billData.GetBillByIdAsync(billId);
        }
    }
}