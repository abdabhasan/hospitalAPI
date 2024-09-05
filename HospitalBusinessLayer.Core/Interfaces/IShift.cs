using HospitalDataLayer.Infrastructure.DTOs.Shift;

namespace HospitalBusinessLayer.Core.Interfaces
{
    public interface IShift
    {
        Task<List<ShiftDTO>> GetAllShiftsAsync();
        Task<List<ShiftDTO>> GetStaffShiftsByStaffIdAsync(int staffId);
        Task<bool> CreateShiftAsync(CreateShiftDTO shift);
        Task<bool> UpdateShiftByIdAsync(int shiftId, UpdateShiftDTO shift);
        Task<bool> DeleteShiftByIdAsync(int shiftId);
    }
}