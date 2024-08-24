
using HospitalDataLayer.Infrastructure.DTOs.Shift;
using HospitalDataLayer.Infrastructure.Interfaces;

namespace HospitalBusinessLayer.Core
{
    public class clsShift
    {

        private readonly IShiftData _shiftData;

        public clsShift(IShiftData shiftData)
        {
            _shiftData = shiftData;
        }


        public async Task<List<ShiftDTO>> GetAllShiftsAsync()
        {
            return await _shiftData.GetAllShiftsAsync();
        }
        public async Task<List<ShiftDTO>> GetStaffShiftsByStaffIdAsync(int staffId)
        {
            return await _shiftData.GetStaffShiftsByStaffIdAsync(staffId);
        }
        public async Task<bool> CreateShiftAsync(CreateShiftDTO shift)
        {
            return await _shiftData.CreateShiftAsync(shift);
        }
        public async Task<bool> UpdateShiftByIdAsync(int shiftId, UpdateShiftDTO shift)
        {
            return await _shiftData.UpdateShiftByIdAsync(shiftId, shift);
        }
        public async Task<bool> DeleteShiftByIdAsync(int shiftId)
        {
            return await _shiftData.DeleteShiftByIdAsync(shiftId);
        }


    }
}