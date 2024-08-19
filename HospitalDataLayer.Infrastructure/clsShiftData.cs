using HospitalDataLayer.Infrastructure.DTOs.Shift;
using Npgsql;

namespace HospitalDataLayer.Infrastructure
{

    public class clsShiftData
    {
        private static readonly string _connectionString = clsDataLayerSettings.ConnectionString;


        public static async Task<List<ShiftDTO>> GetStaffShiftsByStaffIdAsync(int staffId)
        {
            var shifts = new List<ShiftDTO>();

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string query = "SELECT * FROM get_shift_by_staff_id(@StaffId::INT)";

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
                                        Date = reader.GetDateTime(1),
                                        StartTime = reader.GetTimeSpan(2),
                                        EndTime = reader.GetTimeSpan(3),
                                        Role = reader.GetString(4),
                                        Notes = reader.IsDBNull(5) ? null : reader.GetString(5)
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