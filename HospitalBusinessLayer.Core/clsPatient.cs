using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HospitalDataLayer.Infrastructure;
using HospitalDataLayer.Infrastructure.DTOs;

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


    }
}