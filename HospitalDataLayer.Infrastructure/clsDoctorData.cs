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


        public static async Task<DoctorDTO> GetDoctorByIdAsync(int doctorId)
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
                Console.WriteLine($"Database error occurred while fetching the Doctor by ID: {npgsqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching the Doctor by ID: {ex.Message}");
            }

            return doctor;
        }

        public static async Task<int> CreateDoctorAsync(CreateDoctorDTO doctor)
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
                Console.WriteLine($"Database error occurred while creating the Doctor: {npgsqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while creating the Doctor: {ex.Message}");
            }

            return newDoctorId;
        }

    }
}