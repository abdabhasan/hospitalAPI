
using HospitalDataLayer.Infrastructure.DTOs;
using HospitalDataLayer.Infrastructure.DTOs.Patient;

namespace HospitalDataLayer.Infrastructure.Interfaces
{
    public interface IPatientData
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