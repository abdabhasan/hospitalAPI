using HospitalDataLayer.Infrastructure.DTOs.Doctor;
using Npgsql;

namespace HospitalDataLayer.Infrastructure
{
    public class clsDoctorData
    {
        private static readonly string _connectionString = clsDataLayerSettings.ConnectionString;

        public static async Task<List<DoctorDTO>> GetAllDoctorsAsync()
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
                Console.WriteLine($"Database error occurred while fetching Doctors.: {npgsqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching Doctors.: {ex.Message}");
            }

            return DoctorsList;

        }

    }
}