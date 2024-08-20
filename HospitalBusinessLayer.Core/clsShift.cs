
using HospitalDataLayer.Infrastructure;
using HospitalDataLayer.Infrastructure.DTOs.Shift;

namespace HospitalBusinessLayer.Core
{
    public class clsShift
    {
        public static async Task<List<ShiftDTO>> GetAllShiftsAsync()
        {
            return await clsShiftData.GetAllShiftsAsync();
        }
        public static async Task<List<ShiftDTO>> GetStaffShiftsByStaffIdAsync(int staffId)
        {
            return await clsShiftData.GetStaffShiftsByStaffIdAsync(staffId);
        }
        public static async Task<bool> CreateShiftAsync(CreateShiftDTO shift)
        {
            return await clsShiftData.CreateShiftAsync(shift);
        }
        public static async Task<bool> UpdateShiftByIdAsync(int shiftId, UpdateShiftDTO shift)
        {
            return await clsShiftData.UpdateShiftByIdAsync(shiftId, shift);
        }
        public static async Task<bool> DeleteShiftByIdAsync(int shiftId)
        {
            return await clsShiftData.DeleteShiftByIdAsync(shiftId);
        }


    }
}