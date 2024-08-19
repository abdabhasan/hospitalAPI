
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
    }
}