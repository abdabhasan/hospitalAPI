using HospitalDataLayer.Infrastructure.DTOs.Doctor;
using HospitalDataLayer.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace HospitalDataLayer.Infrastructure
{
    public class clsDoctorData : IDoctorData
    {
        private readonly string _connectionString;
        private readonly ILogger<clsDoctorData> _logger;

        public clsDoctorData(string connectionString, ILogger<clsDoctorData> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<List<DoctorDTO>> GetAllDoctorsAsync()
        {
            var DoctorsList = new List<DoctorDTO>();

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {

                    await conn.OpenAsync();

                    string query = "SELECT * FROM get_all_doctors()";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var doctor = new DoctorDTO
                            {
                                Id = reader.GetInt32(0),
                                FullName = reader.GetString(1),
                                Gender = reader.GetString(2),
                                Address = reader.GetString(3),
                                Phone = reader.GetString(4),
                                Email = reader.GetString(5),
                                BirthDate = reader.GetDateTime(6),
                                Specialization = reader.GetString(7),
                                OfficeNumber = reader.IsDBNull(8) ? null : reader.GetString(8),
                                YearsOfExperience = reader.GetInt32(9),
                                Qualifications = reader.GetString(10),
                            };
                            DoctorsList.Add(doctor);
                        }
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                _logger.LogError(npgsqlEx, "Database error occurred while fetching doctors.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching doctors.");
            }

            return DoctorsList;

        }


        public async Task<DoctorDTO> GetDoctorByIdAsync(int doctorId)
        {
            DoctorDTO doctor = null;

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string query = "SELECT * FROM get_doctor_by_id(@Id)";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("Id", doctorId);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                doctor = new DoctorDTO
                                {
                                    Id = reader.GetInt32(0),
                                    FullName = reader.GetString(1),
                                    Gender = reader.GetString(2),
                                    Address = reader.GetString(3),
                                    Phone = reader.GetString(4),
                                    Email = reader.GetString(5),
                                    BirthDate = reader.GetDateTime(6),
                                    Specialization = reader.GetString(7),
                                    OfficeNumber = reader.IsDBNull(8) ? null : reader.GetString(8),
                                    YearsOfExperience = reader.GetInt32(9),
                                    Qualifications = reader.GetString(10),
                                };
                            }
                        }
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                _logger.LogError(npgsqlEx, "Database error occurred while fetching the Doctor by ID");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the Doctor by ID");
            }

            return doctor;
        }


        public async Task<int> CreateDoctorAsync(CreateDoctorDTO doctor)
        {
            int newDoctorId = 0;

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string query = "SELECT create_doctor(" +
                                   "@FirstName::TEXT, " +
                                   "@LastName::TEXT, " +
                                   "@Gender::TEXT, " +
                                   "@Address::TEXT, " +
                                   "@Phone::TEXT, " +
                                   "@Email::TEXT, " +
                                   "@BirthDate::DATE, " +
                                   "@Specialization::VARCHAR, " +
                                   "@OfficeNumber::VARCHAR, " +
                                   "@YearsOfExperience::INT, " +
                                   "@Qualifications::TEXT )";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("FirstName", doctor.FirstName);
                        cmd.Parameters.AddWithValue("LastName", doctor.LastName);
                        cmd.Parameters.AddWithValue("Gender", doctor.Gender);
                        cmd.Parameters.AddWithValue("Address", doctor.Address);
                        cmd.Parameters.AddWithValue("Phone", doctor.Phone);
                        cmd.Parameters.AddWithValue("Email", doctor.Email);
                        cmd.Parameters.AddWithValue("BirthDate", doctor.BirthDate);
                        cmd.Parameters.AddWithValue("Specialization", doctor.Specialization);
                        cmd.Parameters.AddWithValue("OfficeNumber", doctor.OfficeNumber);
                        cmd.Parameters.AddWithValue("YearsOfExperience", doctor.YearsOfExperience);
                        cmd.Parameters.AddWithValue("Qualifications", doctor.Qualifications);

                        newDoctorId = (int)await cmd.ExecuteScalarAsync();
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                _logger.LogError(npgsqlEx, "Database error occurred while creating the Doctor ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the Doctor ");
            }

            return newDoctorId;
        }


        public async Task<string> GetDoctorOfficeNumberAsync(int doctorId)
        {
            string officeNumber = string.Empty;

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string query = "SELECT get_doctor_office_number(@DoctorId::INT)";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("DoctorId", doctorId);

                        var result = await cmd.ExecuteScalarAsync();

                        if (result != null && result != DBNull.Value)
                        {
                            officeNumber = result.ToString();

                            if (officeNumber.StartsWith("Error:"))
                            {
                                return officeNumber;
                            }
                        }
                        else
                        {
                            officeNumber = "Error: An unexpected error occurred.";
                        }
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                _logger.LogError(npgsqlEx, "Database error occurred while retrieving the doctor's office number ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the doctor's office number ");
            }

            return officeNumber;
        }


        public async Task<bool> UpdateDoctorAsync(int doctorId, UpdateDoctorDTO updateDoctorDto)
        {
            bool isUpdated = false;

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string query = "SELECT update_doctor(" +
                                   "@DoctorId::INT, " +
                                   "@FirstName::TEXT, " +
                                   "@LastName::TEXT, " +
                                   "@Gender::TEXT, " +
                                   "@Address::TEXT, " +
                                   "@Phone::TEXT, " +
                                   "@Email::TEXT, " +
                                   "@BirthDate::DATE, " +
                                   "@Specialization::VARCHAR, " +
                                   "@OfficeNumber::VARCHAR, " +
                                   "@YearsOfExperience::INT, " +
                                   "@Qualifications::TEXT )";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("DoctorId", doctorId);
                        cmd.Parameters.AddWithValue("FirstName", updateDoctorDto.FirstName);
                        cmd.Parameters.AddWithValue("LastName", updateDoctorDto.LastName);
                        cmd.Parameters.AddWithValue("Gender", updateDoctorDto.Gender);
                        cmd.Parameters.AddWithValue("Address", updateDoctorDto.Address);
                        cmd.Parameters.AddWithValue("Phone", updateDoctorDto.Phone);
                        cmd.Parameters.AddWithValue("Email", updateDoctorDto.Email);
                        cmd.Parameters.AddWithValue("BirthDate", updateDoctorDto.BirthDate);
                        cmd.Parameters.AddWithValue("Specialization", updateDoctorDto.Specialization);
                        cmd.Parameters.AddWithValue("OfficeNumber", updateDoctorDto.OfficeNumber);
                        cmd.Parameters.AddWithValue("YearsOfExperience", updateDoctorDto.YearsOfExperience);
                        cmd.Parameters.AddWithValue("Qualifications", updateDoctorDto.Qualifications);

                        var result = await cmd.ExecuteScalarAsync();
                        isUpdated = (result != null && (bool)result);
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                _logger.LogError(npgsqlEx, "Database error occurred while updating the Doctor ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the Doctor ");
            }

            return isUpdated;
        }


        public async Task<bool> DeleteDoctorAsync(int doctorId)
        {
            bool isDeleted = false;

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string query = "SELECT delete_doctor(@DoctorId::INT)";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("DoctorId", doctorId);

                        var result = await cmd.ExecuteScalarAsync();
                        isDeleted = (result != null && (bool)result);
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                _logger.LogError(npgsqlEx, "Database error occurred while deleting the Doctor ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the Doctor ");
            }

            return isDeleted;
        }

    }
}