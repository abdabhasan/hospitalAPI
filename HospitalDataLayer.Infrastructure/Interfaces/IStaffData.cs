
using HospitalDataLayer.Infrastructure.DTOs.Staff;

namespace HospitalDataLayer.Infrastructure.Interfaces
{
    public interface IStaffData
    {
        Task<List<StaffDTO>> GetAllStaffAsync();
        Task<StaffDTO> GetStaffByIdAsync(int staffId);
        Task<int> CreateStaffAsync(CreateStaffDTO staff);
        Task<bool> UpdateStaffByIdAsync(int staffId, UpdateStaffDTO updateStaffDto);
        Task<bool> DeleteStaffByIdAsync(int staffId);
    }
}