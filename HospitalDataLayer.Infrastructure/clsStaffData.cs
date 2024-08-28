using HospitalDataLayer.Infrastructure.DTOs.Staff;
using HospitalDataLayer.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace HospitalDataLayer.Infrastructure
{
    public class clsStaffData : IStaffData
    {
        private readonly string _connectionString;
        private readonly ILogger<clsStaffData> _logger;

        public clsStaffData(string connectionString, ILogger<clsStaffData> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<List<StaffDTO>> GetAllStaffAsync()
        {
            var StaffList = new List<StaffDTO>();

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {

                    await conn.OpenAsync();

                    string query = "SELECT * FROM get_all_staff()";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var staff = new StaffDTO
                            {
                                Id = reader.GetInt32(0),
                                FullName = reader.GetString(1),
                                Gender = reader.GetString(2),
                                Address = reader.GetString(3),
                                Phone = reader.GetString(4),
                                Email = reader.GetString(5),
                                BirthDate = reader.GetDateTime(6),
                                Role = reader.GetString(7),
                                Qualifications = reader.GetString(8),
                            };
                            StaffList.Add(staff);
                        }
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                _logger.LogError(npgsqlEx, "Database error occurred while fetching Staff. ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching Staff. ");
            }

            return StaffList;

        }


        public async Task<StaffDTO> GetStaffByIdAsync(int staffId)
        {
            StaffDTO staff = null;

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string query = "SELECT * FROM get_staff_by_id(@Id)";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("Id", staffId);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                staff = new StaffDTO
                                {
                                    Id = reader.GetInt32(0),
                                    FullName = reader.GetString(1),
                                    Gender = reader.GetString(2),
                                    Address = reader.GetString(3),
                                    Phone = reader.GetString(4),
                                    Email = reader.GetString(5),
                                    BirthDate = reader.GetDateTime(6),
                                    Role = reader.GetString(7),
                                    Qualifications = reader.GetString(8),
                                };
                            }
                        }
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                _logger.LogError(npgsqlEx, "Database error occurred while fetching the Staff by ID ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the Staff by ID ");
            }

            return staff;
        }


        public async Task<int> CreateStaffAsync(CreateStaffDTO staff)
        {
            int newStaffId = 0;

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string query = "SELECT create_staff(" +
                                   "@FirstName::TEXT, " +
                                   "@LastName::TEXT, " +
                                   "@Gender::TEXT, " +
                                   "@Address::TEXT, " +
                                   "@Phone::TEXT, " +
                                   "@Email::TEXT, " +
                                   "@BirthDate::DATE, " +
                                   "@Role::VARCHAR, " +
                                   "@Qualifications::TEXT )";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("FirstName", staff.FirstName);
                        cmd.Parameters.AddWithValue("LastName", staff.LastName);
                        cmd.Parameters.AddWithValue("Gender", staff.Gender);
                        cmd.Parameters.AddWithValue("Address", staff.Address);
                        cmd.Parameters.AddWithValue("Phone", staff.Phone);
                        cmd.Parameters.AddWithValue("Email", staff.Email);
                        cmd.Parameters.AddWithValue("BirthDate", staff.BirthDate);
                        cmd.Parameters.AddWithValue("Role", staff.Role);
                        cmd.Parameters.AddWithValue("Qualifications", staff.Qualifications);

                        newStaffId = (int)await cmd.ExecuteScalarAsync();
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                _logger.LogError(npgsqlEx, "Database error occurred while creating the Staff ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the Staff ");
            }

            return newStaffId;
        }


        public async Task<bool> UpdateStaffByIdAsync(int staffId, UpdateStaffDTO updateStaffDto)
        {
            bool isUpdated = false;

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string query = "SELECT update_staff_by_id(" +
                                   "@StaffId::INT, " +
                                   "@FirstName::TEXT, " +
                                   "@LastName::TEXT, " +
                                   "@Gender::TEXT, " +
                                   "@Address::TEXT, " +
                                   "@Phone::TEXT, " +
                                   "@Email::TEXT, " +
                                   "@BirthDate::DATE, " +
                                   "@Role::VARCHAR, " +
                                   "@Qualifications::TEXT )";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("StaffId", staffId);
                        cmd.Parameters.AddWithValue("FirstName", updateStaffDto.FirstName);
                        cmd.Parameters.AddWithValue("LastName", updateStaffDto.LastName);
                        cmd.Parameters.AddWithValue("Gender", updateStaffDto.Gender);
                        cmd.Parameters.AddWithValue("Address", updateStaffDto.Address);
                        cmd.Parameters.AddWithValue("Phone", updateStaffDto.Phone);
                        cmd.Parameters.AddWithValue("Email", updateStaffDto.Email);
                        cmd.Parameters.AddWithValue("BirthDate", updateStaffDto.BirthDate);
                        cmd.Parameters.AddWithValue("Role", updateStaffDto.Role);
                        cmd.Parameters.AddWithValue("Qualifications", updateStaffDto.Qualifications);

                        var result = await cmd.ExecuteScalarAsync();
                        isUpdated = (result != null && (bool)result);
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                _logger.LogError(npgsqlEx, "Database error occurred while updating the Staff ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the Staff ");
            }

            return isUpdated;
        }


        public async Task<bool> DeleteStaffByIdAsync(int staffId)
        {
            bool isDeleted = false;

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string query = "SELECT delete_staff_by_id(@StaffId::INT)";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("StaffId", staffId);

                        var result = await cmd.ExecuteScalarAsync();
                        isDeleted = (result != null && (bool)result);
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                _logger.LogError(npgsqlEx, "Database error occurred while deleting the Staff ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the Staff ");
            }

            return isDeleted;
        }

    }
}