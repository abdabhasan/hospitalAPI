using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs.Staff;
using HospitalDataLayer.Infrastructure.Interfaces;

namespace HospitalBusinessLayer.Core
{
    public class clsStaff : IStaff
    {

        private readonly IStaffData _staffData;

        public clsStaff(IStaffData staffData)
        {
            _staffData = staffData;

        }

        public async Task<List<StaffDTO>> GetAllStaffAsync()
        {
            return await _staffData.GetAllStaffAsync();
        }
        public async Task<StaffDTO> GetStaffByIdAsync(int staffId)
        {
            return await _staffData.GetStaffByIdAsync(staffId);
        }
        public async Task<int> CreateStaffAsync(CreateStaffDTO staff)
        {
            return await _staffData.CreateStaffAsync(staff);
        }
        public async Task<bool> UpdateStaffByIdAsync(int staffId, UpdateStaffDTO updateStaffDto)
        {
            return await _staffData.UpdateStaffByIdAsync(staffId, updateStaffDto);
        }
        public async Task<bool> DeleteStaffByIdAsync(int staffId)
        {
            return await _staffData.DeleteStaffByIdAsync(staffId);
        }

    }
}