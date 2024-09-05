
using HospitalDataLayer.Infrastructure.DTOs;
using HospitalDataLayer.Infrastructure.DTOs.Patient;

namespace HospitalBusinessLayer.Core.Interfaces
{
    public interface IPatient
    {
        Task<List<PatientDTO>> GetAllPatientsAsync();
        Task<PatientDTO> GetPatientByIdAsync(int PatientId);
        Task<int> CreatePatientAsync(CreatePatientDTO Patient);
        Task<bool> DeletePatientAsync(int patientId);
        Task<bool> UpdatePatientAsync(int patientId, UpdatePatientDTO updatePatientDto);
        Task<string> GetPatientMedicalHistoryAsync(int patientId);
        Task<string> GetPatientAllergiesAsync(int patientId);
    }
}