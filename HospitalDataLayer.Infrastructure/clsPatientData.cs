using HospitalDataLayer.Infrastructure.DTOs;
using Npgsql;
using HospitalDataLayer.Infrastructure.DTOs.Patient;
using HospitalDataLayer.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace HospitalDataLayer.Infrastructure
{
    public class clsPatientData : IPatientData
    {

        private readonly string _connectionString;
        private readonly ILogger<clsPatientData> _logger;

        public clsPatientData(string connectionString, ILogger<clsPatientData> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<List<PatientDTO>> GetAllPatientsAsync()
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
                                InsuranceProvider = reader.IsDBNull(9) ? null : reader.GetString(9),
                                InsurancePolicyNumber = reader.IsDBNull(10) ? null : reader.GetString(10),
                                MedicalHistory = reader.IsDBNull(11) ? null : reader.GetString(11),
                                Allergies = reader.IsDBNull(12) ? null : reader.GetString(12)
                            };
                            PatientsList.Add(patient);
                        }
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                _logger.LogError(npgsqlEx, "Database error occurred while fetching patients. ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching patients. ");
            }

            return PatientsList;

        }


        public async Task<PatientDTO> GetPatientByIdAsync(int PatientId)
        {
            PatientDTO? Patient = null;

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
                                    InsuranceProvider = reader.IsDBNull(9) ? null : reader.GetString(9),
                                    InsurancePolicyNumber = reader.IsDBNull(10) ? null : reader.GetString(10),
                                    MedicalHistory = reader.IsDBNull(11) ? null : reader.GetString(11),
                                    Allergies = reader.IsDBNull(12) ? null : reader.GetString(12)
                                };
                            }
                        }
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                _logger.LogError(npgsqlEx, "Database error occurred while fetching the patient by ID ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the patient by ID ");
            }

            return Patient!;
        }


        public async Task<int> CreatePatientAsync(CreatePatientDTO Patient)
        {
            int NewPatientId = 0;

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string query = "SELECT create_patient(" +
                                   "@FirstName::TEXT, " +
                                   "@LastName::TEXT, " +
                                   "@Gender::TEXT, " +
                                   "@Address::TEXT, " +
                                   "@Phone::TEXT, " +
                                   "@Email::TEXT, " +
                                   "@BirthDate::DATE, " +
                                   "@EmergencyContactName::TEXT, " +
                                   "@EmergencyContactPhone::TEXT, " +
                                   "@InsuranceProvider::TEXT, " +
                                   "@InsurancePolicyNumber::TEXT, " +
                                   "@MedicalHistory::TEXT, " +
                                   "@Allergies::TEXT)";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("FirstName", Patient.FirstName);
                        cmd.Parameters.AddWithValue("LastName", Patient.LastName);
                        cmd.Parameters.AddWithValue("Gender", Patient.Gender);
                        cmd.Parameters.AddWithValue("Address", Patient.Address);
                        cmd.Parameters.AddWithValue("Phone", Patient.Phone);
                        cmd.Parameters.AddWithValue("Email", Patient.Email);
                        cmd.Parameters.AddWithValue("BirthDate", Patient.BirthDate);
                        cmd.Parameters.AddWithValue("EmergencyContactName", Patient.EmergencyContactName);
                        cmd.Parameters.AddWithValue("EmergencyContactPhone", Patient.EmergencyContactPhone);
                        cmd.Parameters.AddWithValue("InsuranceProvider", Patient.InsuranceProvider ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("InsurancePolicyNumber", Patient.InsurancePolicyNumber ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("MedicalHistory", Patient.MedicalHistory ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("Allergies", Patient.Allergies ?? (object)DBNull.Value);

                        NewPatientId = (await cmd.ExecuteScalarAsync() as int?) ?? 0;
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                _logger.LogError(npgsqlEx, "Database error occurred while creating the patient ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the patient ");
            }

            return NewPatientId;
        }


        public async Task<bool> DeletePatientAsync(int patientId)
        {
            bool isDeleted = false;

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string query = "SELECT delete_patient(@PatientId::INT)";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("PatientId", patientId);

                        var result = await cmd.ExecuteScalarAsync();
                        isDeleted = (result != null && (bool)result);
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                _logger.LogError(npgsqlEx, "Database error occurred while deleting the patient ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the patient ");
            }

            return isDeleted;
        }


        public async Task<bool> UpdatePatientAsync(int patientId, UpdatePatientDTO updatePatientDto)
        {
            bool isUpdated = false;

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string query = "SELECT update_patient(" +
                                   "@PatientId::INT, " +
                                   "@FirstName::TEXT, " +
                                   "@LastName::TEXT, " +
                                   "@Gender::TEXT, " +
                                   "@Address::TEXT, " +
                                   "@Phone::TEXT, " +
                                   "@Email::TEXT, " +
                                   "@BirthDate::DATE, " +
                                   "@EmergencyContactName::TEXT, " +
                                   "@EmergencyContactPhone::TEXT, " +
                                   "@InsuranceProvider::TEXT, " +
                                   "@InsurancePolicyNumber::TEXT, " +
                                   "@MedicalHistory::TEXT, " +
                                   "@Allergies::TEXT)";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("PatientId", patientId);
                        cmd.Parameters.AddWithValue("FirstName", updatePatientDto.FirstName);
                        cmd.Parameters.AddWithValue("LastName", updatePatientDto.LastName);
                        cmd.Parameters.AddWithValue("Gender", updatePatientDto.Gender);
                        cmd.Parameters.AddWithValue("Address", updatePatientDto.Address);
                        cmd.Parameters.AddWithValue("Phone", updatePatientDto.Phone);
                        cmd.Parameters.AddWithValue("Email", updatePatientDto.Email);
                        cmd.Parameters.AddWithValue("BirthDate", updatePatientDto.BirthDate);
                        cmd.Parameters.AddWithValue("EmergencyContactName", updatePatientDto.EmergencyContactName);
                        cmd.Parameters.AddWithValue("EmergencyContactPhone", updatePatientDto.EmergencyContactPhone);
                        cmd.Parameters.AddWithValue("InsuranceProvider", updatePatientDto.InsuranceProvider ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("InsurancePolicyNumber", updatePatientDto.InsurancePolicyNumber ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("MedicalHistory", updatePatientDto.MedicalHistory ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("Allergies", updatePatientDto.Allergies ?? (object)DBNull.Value);

                        var result = await cmd.ExecuteScalarAsync();
                        isUpdated = (result != null && (bool)result);
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                _logger.LogError(npgsqlEx, "Database error occurred while updating the patient ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the patient ");
            }

            return isUpdated;
        }


        public async Task<string> GetPatientMedicalHistoryAsync(int patientId)
        {
            string medicalHistory = string.Empty;

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string query = "SELECT get_patient_medical_history(@PatientId::INT)";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("PatientId", patientId);

                        var result = await cmd.ExecuteScalarAsync();

                        if (result != null && result != DBNull.Value)
                        {
                            medicalHistory = result.ToString() ?? string.Empty;

                            if (medicalHistory.StartsWith("Error:"))
                            {
                                return medicalHistory;
                            }
                        }
                        else
                        {
                            medicalHistory = "Error: An unexpected error occurred.";
                        }
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                _logger.LogError(npgsqlEx, "Database error occurred while retrieving the patient's medical history ");
                medicalHistory = "An error occurred while retrieving the medical history.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the patient's medical history ");
                medicalHistory = "An error occurred while retrieving the medical history.";
            }

            return medicalHistory;
        }


        public async Task<string> GetPatientAllergiesAsync(int patientId)
        {
            string allergies = string.Empty;

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string query = "SELECT get_patient_allergies(@PatientId::INT)";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("PatientId", patientId);

                        var result = await cmd.ExecuteScalarAsync();

                        if (result != null && result != DBNull.Value)
                        {
                            allergies = result.ToString() ?? string.Empty;

                            if (allergies.StartsWith("Error:"))
                            {
                                return allergies;
                            }
                        }
                        else
                        {
                            allergies = "Error: An unexpected error occurred.";
                        }
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                _logger.LogError(npgsqlEx, "Database error occurred while retrieving the patient's allergies ");
                allergies = "An error occurred while retrieving the allergies.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the patient's allergies ");
                allergies = "An error occurred while retrieving the allergies.";
            }

            return allergies;
        }


    }
}