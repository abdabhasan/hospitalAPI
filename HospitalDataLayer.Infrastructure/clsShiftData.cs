using HospitalDataLayer.Infrastructure.DTOs.Shift;
using HospitalDataLayer.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace HospitalDataLayer.Infrastructure
{

    public class clsShiftData : IShiftData
    {
        private readonly string _connectionString;
        private readonly ILogger<clsShiftData> _logger;

        public clsShiftData(string connectionString, ILogger<clsShiftData> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }


        public async Task<List<ShiftDTO>> GetAllShiftsAsync()
        {
            var shifts = new List<ShiftDTO>();

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string query = "SELECT * FROM get_all_shifts()";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    var shift = new ShiftDTO
                                    {
                                        Id = reader.GetInt32(0),
                                        StaffName = reader.GetString(1),
                                        Phone = reader.GetString(2),
                                        Date = reader.GetDateTime(3),
                                        StartTime = reader.GetTimeSpan(4),
                                        EndTime = reader.GetTimeSpan(5),
                                        Role = reader.GetString(6),
                                        Notes = reader.IsDBNull(7) ? null : reader.GetString(7)
                                    };

                                    shifts.Add(shift);
                                }
                            }
                            else
                            {
                                Console.WriteLine("No shifts found.");
                            }
                        }
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                _logger.LogError(npgsqlEx, "Database error occurred while retrieving shifts ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving shifts ");
            }

            return shifts;
        }


        public async Task<List<ShiftDTO>> GetStaffShiftsByStaffIdAsync(int staffId)
        {
            var shifts = new List<ShiftDTO>();

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string query = "SELECT * FROM get_staff_shifts_by_staff_id(@StaffId::INT)";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("StaffId", staffId);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    var shift = new ShiftDTO
                                    {
                                        Id = reader.GetInt32(0),
                                        StaffName = reader.GetString(1),
                                        Phone = reader.GetString(2),
                                        Date = reader.GetDateTime(3),
                                        StartTime = reader.GetTimeSpan(4),
                                        EndTime = reader.GetTimeSpan(5),
                                        Role = reader.GetString(6),
                                        Notes = reader.IsDBNull(7) ? null : reader.GetString(7)
                                    };

                                    shifts.Add(shift);
                                }
                            }
                            else
                            {
                                Console.WriteLine("No shifts found for the provided staff ID.");
                            }
                        }
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                _logger.LogError(npgsqlEx, "Database error occurred while retrieving the staff's shift ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the staff's shift ");
            }

            return shifts;
        }

        public async Task<bool> CreateShiftAsync(CreateShiftDTO shift)
        {
            bool isSuccess = false;

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string query = "SELECT create_shift(" +
                                    "@StaffId::int, " +
                                    "@Date::date, " +
                                    "@StartTime::time, " +
                                    "@EndTime::time, " +
                                    "@Role::varchar, " +
                                    "@Notes::text)";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StaffId", shift.StaffId);
                        cmd.Parameters.AddWithValue("@Date", shift.Date.Date);
                        cmd.Parameters.AddWithValue("@StartTime", shift.StartTime);
                        cmd.Parameters.AddWithValue("@EndTime", shift.EndTime);
                        cmd.Parameters.AddWithValue("@Role", shift.Role);
                        cmd.Parameters.AddWithValue("@Notes", (object)shift.Notes ?? DBNull.Value);

                        var result = await cmd.ExecuteScalarAsync();

                        if (result != null && Convert.ToInt32(result) > 0)
                        {
                            isSuccess = true;
                        }
                        else
                        {
                            Console.WriteLine("Failed to create shift.");
                        }
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                _logger.LogError(npgsqlEx, "Database error occurred while creating shift ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating shift ");
            }

            return isSuccess;
        }


        public async Task<bool> UpdateShiftByIdAsync(int shiftId, UpdateShiftDTO shift)
        {
            bool isSuccess = false;

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string query = "SELECT update_shift_by_id(" +
                                    "@ShiftId::int, " +
                                    "@StaffId::int, " +
                                    "@Date::date, " +
                                    "@StartTime::time, " +
                                    "@EndTime::time, " +
                                    "@Role::varchar, " +
                                    "@Notes::text)";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ShiftId", shiftId);
                        cmd.Parameters.AddWithValue("@StaffId", shift.StaffId);
                        cmd.Parameters.AddWithValue("@Date", shift.Date.Date);
                        cmd.Parameters.AddWithValue("@StartTime", shift.StartTime);
                        cmd.Parameters.AddWithValue("@EndTime", shift.EndTime);
                        cmd.Parameters.AddWithValue("@Role", shift.Role);
                        cmd.Parameters.AddWithValue("@Notes", (object)shift.Notes ?? DBNull.Value);

                        var result = await cmd.ExecuteScalarAsync();

                        if (result != null && Convert.ToInt32(result) > 0)
                        {
                            isSuccess = true;
                        }
                        else
                        {
                            Console.WriteLine("Failed to update shift.");
                        }
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                _logger.LogError(npgsqlEx, "Database error occurred while updating shift ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating shift ");
            }

            return isSuccess;
        }


        public async Task<bool> DeleteShiftByIdAsync(int shiftId)
        {
            bool isSuccess = false;

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string query = "SELECT delete_shift_by_id(@ShiftId::int)";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ShiftId", shiftId);

                        var result = await cmd.ExecuteScalarAsync();

                        if (result != null && Convert.ToBoolean(result))
                        {
                            isSuccess = true;
                        }
                        else
                        {
                            Console.WriteLine("Failed to delete shift.");
                        }
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                _logger.LogError(npgsqlEx, "Database error occurred while deleting shift ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting shift ");
            }

            return isSuccess;
        }


    }
}