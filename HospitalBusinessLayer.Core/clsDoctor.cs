using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs.Doctor;
using HospitalDataLayer.Infrastructure.Interfaces;

namespace HospitalBusinessLayer.Core;

public class clsDoctor : IDoctor
{
    private readonly IDoctorData _doctorData;

    public clsDoctor(IDoctorData doctorData)
    {
        _doctorData = doctorData;
    }

    public async Task<List<DoctorDTO>> GetAllDoctorsAsync()
    {
        return await _doctorData.GetAllDoctorsAsync();
    }
    public async Task<DoctorDTO> GetDoctorByIdAsync(int doctorId)
    {
        return await _doctorData.GetDoctorByIdAsync(doctorId);
    }
    public async Task<int> CreateDoctorAsync(CreateDoctorDTO doctor)
    {
        return await _doctorData.CreateDoctorAsync(doctor);
    }
    public async Task<string> GetDoctorOfficeNumberAsync(int doctorId)
    {
        return await _doctorData.GetDoctorOfficeNumberAsync(doctorId);
    }
    public async Task<bool> UpdateDoctorAsync(int doctorId, UpdateDoctorDTO updateDoctorDto)
    {
        return await _doctorData.UpdateDoctorAsync(doctorId, updateDoctorDto);
    }
    public async Task<bool> DeleteDoctorAsync(int doctorId)
    {
        return await _doctorData.DeleteDoctorAsync(doctorId);
    }

}
