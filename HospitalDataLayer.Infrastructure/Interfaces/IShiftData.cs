
using HospitalDataLayer.Infrastructure.DTOs.Shift;

namespace HospitalDataLayer.Infrastructure.Interfaces
{
    public interface IShiftData
    {
        Task<List<ShiftDTO>> GetAllShiftsAsync();
        Task<List<ShiftDTO>> GetStaffShiftsByStaffIdAsync(int staffId);
        Task<bool> CreateShiftAsync(CreateShiftDTO shift);
        Task<bool> UpdateShiftByIdAsync(int shiftId, UpdateShiftDTO shift);
        Task<bool> DeleteShiftByIdAsync(int shiftId);
    }
}