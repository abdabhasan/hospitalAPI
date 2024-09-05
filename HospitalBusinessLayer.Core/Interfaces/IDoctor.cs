using HospitalDataLayer.Infrastructure.DTOs.Doctor;

namespace HospitalBusinessLayer.Core.Interfaces
{
    public interface IDoctor
    {
        Task<List<DoctorDTO>> GetAllDoctorsAsync();
        Task<DoctorDTO> GetDoctorByIdAsync(int doctorId);
        Task<int> CreateDoctorAsync(CreateDoctorDTO doctor);
        Task<string> GetDoctorOfficeNumberAsync(int doctorId);
        Task<bool> UpdateDoctorAsync(int doctorId, UpdateDoctorDTO updateDoctorDto);
        Task<bool> DeleteDoctorAsync(int doctorId);
    }
}