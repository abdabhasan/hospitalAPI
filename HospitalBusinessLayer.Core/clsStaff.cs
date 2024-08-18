using HospitalDataLayer.Infrastructure;
using HospitalDataLayer.Infrastructure.DTOs.Staff;

namespace HospitalBusinessLayer.Core
{
    public class clsStaff
    {
        public static async Task<List<StaffDTO>> GetAllStaffAsync()
        {
            return await clsStaffData.GetAllStaffAsync();
        }
        public static async Task<StaffDTO> GetStaffByIdAsync(int staffId)
        {
            return await clsStaffData.GetStaffByIdAsync(staffId);
        }
        public static async Task<int> CreateStaffAsync(CreateStaffDTO staff)
        {
            return await clsStaffData.CreateStaffAsync(staff);
        }
        public static async Task<bool> UpdateStaffByIdAsync(int staffId, UpdateStaffDTO updateStaffDto)
        {
            return await clsStaffData.UpdateStaffByIdAsync(staffId, updateStaffDto);
        }
        public static async Task<bool> DeleteStaffByIdAsync(int staffId)
        {
            return await clsStaffData.DeleteStaffByIdAsync(staffId);
        }

    }
}