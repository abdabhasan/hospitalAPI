using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HospitalDataLayer.Infrastructure;
using HospitalDataLayer.Infrastructure.DTOs;
using HospitalDataLayer.Infrastructure.DTOs.Patient;

namespace HospitalBusinessLayer.Core
{
    public class clsPatient
    {
        public static async Task<List<PatientDTO>> GetAllPatientsAsync()
        {
            return await clsPatientData.GetAllPatientsAsync();
        }

        public static async Task<PatientDTO> GetPatientByIdAsync(int PatientId)
        {
            return await clsPatientData.GetPatientByIdAsync(PatientId);
        }
        public static async Task<int> CreatePatientAsync(CreatePatientDTO Patient)
        {
            return await clsPatientData.CreatePatientAsync(Patient);
        }
        public static async Task<bool> DeletePatientAsync(int patientId)
        {
            return await clsPatientData.DeletePatientAsync(patientId);
        }

        public static async Task<bool> UpdatePatientAsync(int patientId, UpdatePatientDTO updatePatientDto)
        {
            return await clsPatientData.UpdatePatientAsync(patientId, updatePatientDto);

        }

    }
}