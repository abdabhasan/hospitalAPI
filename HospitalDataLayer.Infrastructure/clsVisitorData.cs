using HospitalDataLayer.Infrastructure.DTOs.Visitor;
using HospitalDataLayer.Infrastructure.Interfaces;
using Npgsql;


namespace HospitalDataLayer.Infrastructure
{
    public class clsVisitorData : IVisitorData
    {

        private readonly string _connectionString;

        public clsVisitorData(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<VisitorDTO>> GetAllVisitorsAsync()
        {
            var visitors = new List<VisitorDTO>();

            try
            {
                await using var conn = new NpgsqlConnection(_connectionString);
                await conn.OpenAsync().ConfigureAwait(false);

                string query = "SELECT visitor_id, visitor_name, " +
                               "patient_name, relation, visit_date, " +
                               "visit_time, notes " +
                               "FROM get_all_visitors()";

                await using var cmd = new NpgsqlCommand(query, conn);
                await using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);

                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    var visitor = new VisitorDTO
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        PatientName = reader.GetString(2),
                        Relation = reader.IsDBNull(3) ? null : reader.GetString(3),
                        VisitDate = reader.GetDateTime(4),
                        VisitTime = reader.GetTimeSpan(5),
                        Notes = reader.IsDBNull(6) ? null : reader.GetString(6)
                    };

                    visitors.Add(visitor);
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                Console.WriteLine($"Database error occurred while retrieving visitors: {npgsqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving visitors: {ex.Message}");
            }

            return visitors;
        }

        public async Task<bool> DeleteVisitorByIdAsync(int visitorId)
        {
            bool isDeleted = false;

            try
            {
                await using var conn = new NpgsqlConnection(_connectionString);
                await conn.OpenAsync().ConfigureAwait(false);

                string query = "SELECT delete_visitor_by_id(@visitor_id)";

                await using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@visitor_id", visitorId);

                var result = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);

                isDeleted = result > 0;
            }
            catch (NpgsqlException npgsqlEx)
            {
                Console.WriteLine($"Database error occurred while deleting visitor: {npgsqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting visitor: {ex.Message}");
            }

            return isDeleted;
        }

        public async Task<IEnumerable<VisitorDTO>> GetVisitorByNameAsync(string visitorName)
        {
            var visitors = new List<VisitorDTO>();

            try
            {
                await using var conn = new NpgsqlConnection(_connectionString);
                await conn.OpenAsync().ConfigureAwait(false);

                string query = "SELECT visitor_id, visitor_name, " +
                               "patient_name, relation, visit_date, " +
                               "visit_time, notes " +
                               "FROM get_visitor_by_name(@visitor_name)";

                await using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@visitor_name", visitorName);

                await using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);

                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    var visitor = new VisitorDTO
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        PatientName = reader.GetString(2),
                        Relation = reader.IsDBNull(3) ? null : reader.GetString(3),
                        VisitDate = reader.GetDateTime(4),
                        VisitTime = reader.GetTimeSpan(5),
                        Notes = reader.IsDBNull(6) ? null : reader.GetString(6)
                    };

                    visitors.Add(visitor);
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                Console.WriteLine($"Database error occurred while retrieving visitor by name: {npgsqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving visitor by name: {ex.Message}");
            }

            return visitors;
        }

    }
}