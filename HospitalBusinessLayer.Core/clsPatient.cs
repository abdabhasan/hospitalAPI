using HospitalDataLayer.Infrastructure.DTOs;
using HospitalDataLayer.Infrastructure.DTOs.Patient;
using HospitalDataLayer.Infrastructure.Interfaces;

namespace HospitalBusinessLayer.Core
{
    public class clsPatient
    {

        private readonly IPatientData _patientData;

        public clsPatient(IPatientData patientData)
        {
            _patientData = patientData;
        }


        public async Task<List<PatientDTO>> GetAllPatientsAsync()
        {
            return await _patientData.GetAllPatientsAsync();
        }

        public async Task<PatientDTO> GetPatientByIdAsync(int PatientId)
        {
            return await _patientData.GetPatientByIdAsync(PatientId);
        }
        public async Task<int> CreatePatientAsync(CreatePatientDTO Patient)
        {
            return await _patientData.CreatePatientAsync(Patient);
        }
        public async Task<bool> DeletePatientAsync(int patientId)
        {
            return await _patientData.DeletePatientAsync(patientId);
        }

        public async Task<bool> UpdatePatientAsync(int patientId, UpdatePatientDTO updatePatientDto)
        {
            return await _patientData.UpdatePatientAsync(patientId, updatePatientDto);
        }

        public async Task<string> GetPatientMedicalHistoryAsync(int patientId)
        {
            return await _patientData.GetPatientMedicalHistoryAsync(patientId);
        }

        public async Task<string> GetPatientAllergiesAsync(int patientId)
        {
            return await _patientData.GetPatientAllergiesAsync(patientId);
        }


    }
}