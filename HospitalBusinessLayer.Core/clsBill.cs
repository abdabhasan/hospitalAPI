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
    }
}