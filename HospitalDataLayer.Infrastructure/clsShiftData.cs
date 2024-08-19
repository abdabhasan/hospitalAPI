using HospitalDataLayer.Infrastructure.DTOs.Shift;
using Npgsql;

namespace HospitalDataLayer.Infrastructure
{

    public class clsShiftData
    {
        private static readonly string _connectionString = clsDataLayerSettings.ConnectionString;



        public static async Task<List<ShiftDTO>> GetAllShiftsAsync()
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
                Console.WriteLine($"Database error occurred while retrieving shifts: {npgsqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving shifts: {ex.Message}");
            }

            return shifts;
        }


        public static async Task<List<ShiftDTO>> GetStaffShiftsByStaffIdAsync(int staffId)
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
                Console.WriteLine($"Database error occurred while retrieving the staff's shift: {npgsqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving the staff's shift: {ex.Message}");
            }

            return shifts;
        }


    }
}