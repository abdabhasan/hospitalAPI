using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HospitalDataLayer.Infrastructure.DTOs;
using Npgsql;
using Microsoft.Extensions.Logging;
using HospitalDataLayer.Infrastructure.DTOs.Patient;

namespace HospitalDataLayer.Infrastructure
{
    public class clsPatientData
    {

        private static readonly string _connectionString = clsDataLayerSettings.ConnectionString;

        public static async Task<List<PatientDTO>> GetAllPatientsAsync()
        {
            var PatientsList = new List<PatientDTO>();

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {

                    await conn.OpenAsync();

                    string query = "SELECT * FROM get_all_patients()";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var patient = new PatientDTO
                            {
                                Id = reader.GetInt32(0),
                                FullName = reader.GetString(1),
                                Gender = reader.GetString(2),
                                Address = reader.GetString(3),
                                Phone = reader.GetString(4),
                                Email = reader.GetString(5),
                                BirthDate = reader.GetDateTime(6),
                                EmergencyContactName = reader.GetString(7),
                                EmergencyContactPhone = reader.GetString(8),
                                InsuranceProvider = reader.GetString(9),
                                InsurancePolicyNumber = reader.GetString(10),
                                MedicalHistory = reader.GetString(11),
                                Allergies = reader.GetString(12)
                            };
                            PatientsList.Add(patient);
                        }
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                Console.WriteLine($"Database error occurred while fetching patients.: {npgsqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching patients.: {ex.Message}");
            }

            return PatientsList;

        }


        public static async Task<PatientDTO> GetPatientByIdAsync(int PatientId)
        {
            PatientDTO Patient = null;

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string query = "SELECT * FROM get_patient_by_id(@Id)";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("Id", PatientId);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                Patient = new PatientDTO
                                {
                                    Id = reader.GetInt32(0),
                                    FullName = reader.GetString(1),
                                    Gender = reader.GetString(2),
                                    Address = reader.GetString(3),
                                    Phone = reader.GetString(4),
                                    Email = reader.GetString(5),
                                    BirthDate = reader.GetDateTime(6),
                                    EmergencyContactName = reader.GetString(7),
                                    EmergencyContactPhone = reader.GetString(8),
                                    InsuranceProvider = reader.GetString(9),
                                    InsurancePolicyNumber = reader.GetString(10),
                                    MedicalHistory = reader.GetString(11),
                                    Allergies = reader.GetString(12)
                                };
                            }
                        }
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                Console.WriteLine($"Database error occurred while fetching the patient by ID: {npgsqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching the patient by ID: {ex.Message}");
            }

            return Patient;
        }

    }
}