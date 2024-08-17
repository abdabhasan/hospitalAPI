using HospitalDataLayer.Infrastructure;
using HospitalDataLayer.Infrastructure.DTOs.Doctor;

namespace HospitalBusinessLayer.Core;

public class clsDoctor
{
    public static async Task<List<DoctorDTO>> GetAllDoctorsAsync()
    {
        return await clsDoctorData.GetAllDoctorsAsync();
    }
    public static async Task<DoctorDTO> GetDoctorByIdAsync(int doctorId)
    {
        return await clsDoctorData.GetDoctorByIdAsync(doctorId);
    }
    public static async Task<int> CreateDoctorAsync(CreateDoctorDTO doctor)
    {
        return await clsDoctorData.CreateDoctorAsync(doctor);
    }
    public static async Task<string> GetDoctorOfficeNumberAsync(int doctorId)
    {
        return await clsDoctorData.GetDoctorOfficeNumberAsync(doctorId);
    }
    public static async Task<bool> DeleteDoctorAsync(int doctorId)
    {
        return await clsDoctorData.DeleteDoctorAsync(doctorId);
    }

}
