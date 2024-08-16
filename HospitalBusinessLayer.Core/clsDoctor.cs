using HospitalDataLayer.Infrastructure;
using HospitalDataLayer.Infrastructure.DTOs.Doctor;

namespace HospitalBusinessLayer.Core;

public class clsDoctor
{
    public static async Task<List<DoctorDTO>> GetAllDoctorsAsync()
    {
        return await clsDoctorData.GetAllDoctorsAsync();
    }
    public static async Task<DoctorDTO> GetDoctorByIdAsync(int patientId)
    {
        return await clsDoctorData.GetDoctorByIdAsync(patientId);
    }
}
