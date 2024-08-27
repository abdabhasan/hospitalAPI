using HospitalDataLayer.Infrastructure.DTOs;
using HospitalDataLayer.Infrastructure.DTOs.InsuranceClaim;
using HospitalDataLayer.Infrastructure.Interfaces;
using Npgsql;

namespace HospitalDataLayer.Infrastructure
{
    public class clsInsuranceClaimData : IInsuranceClaimData
    {

        private readonly string _connectionString;

        public clsInsuranceClaimData(string connectionString)
        {
            _connectionString = connectionString;
        }


        public async Task<IEnumerable<InsuranceClaimDTO>> GetAllInsuranceClaimsAsync()
        {
            var insuranceClaims = new List<InsuranceClaimDTO>();

            try
            {
                await using var conn = new NpgsqlConnection(_connectionString);
                await conn.OpenAsync().ConfigureAwait(false);

                string query = "SELECT claim_id, patient_id, patient_name, " +
                               "insurance_provider, policy_number, claim_date, " +
                               "claim_amount, claim_status, notes " +
                               "FROM get_all_insurance_claims()";

                await using var cmd = new NpgsqlCommand(query, conn);
                await using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);

                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    var insuranceClaim = new InsuranceClaimDTO
                    {
                        Id = reader.GetInt32(0),
                        PatientId = reader.GetInt32(1),
                        PatientName = reader.GetString(2),
                        InsuranceProvider = reader.GetString(3),
                        PolicyNumber = reader.IsDBNull(4) ? null : reader.GetString(4),
                        ClaimDate = reader.GetDateTime(5),
                        ClaimAmount = reader.GetInt32(6),
                        ClaimStatus = reader.GetString(7),
                        Notes = reader.IsDBNull(8) ? null : reader.GetString(8)
                    };

                    insuranceClaims.Add(insuranceClaim);
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                Console.WriteLine($"Database error occurred while retrieving insurance claims: {npgsqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving insurance claims: {ex.Message}");
            }

            return insuranceClaims;
        }

        public async Task<IEnumerable<InsuranceClaimDTO>> GetInsuranceClaimsForPatientByPatientNameAsync(string patientName)
        {
            var insuranceClaims = new List<InsuranceClaimDTO>();

            try
            {
                await using var conn = new NpgsqlConnection(_connectionString);
                await conn.OpenAsync().ConfigureAwait(false);

                string query = "SELECT claim_id, patient_id, patient_name, " +
                               "insurance_provider, policy_number, claim_date, " +
                               "claim_amount, claim_status, notes " +
                               "FROM get_insurance_claims_by_patient_name(@patientName)";

                await using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("patientName", patientName);

                await using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);

                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    var insuranceClaim = new InsuranceClaimDTO
                    {
                        Id = reader.GetInt32(0),
                        PatientId = reader.GetInt32(1),
                        PatientName = reader.GetString(2),
                        InsuranceProvider = reader.GetString(3),
                        PolicyNumber = reader.IsDBNull(4) ? null : reader.GetString(4),
                        ClaimDate = reader.GetDateTime(5),
                        ClaimAmount = reader.GetInt32(6),
                        ClaimStatus = reader.GetString(7),
                        Notes = reader.IsDBNull(8) ? null : reader.GetString(8)
                    };

                    insuranceClaims.Add(insuranceClaim);
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                Console.WriteLine($"Database error occurred while retrieving insurance claims: {npgsqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving insurance claims: {ex.Message}");
            }

            return insuranceClaims;
        }

        public async Task<IEnumerable<InsuranceClaimDTO>> GetInsuranceClaimsForPatientByPatientIdAsync(int patientId)
        {
            var insuranceClaims = new List<InsuranceClaimDTO>();

            try
            {
                await using var conn = new NpgsqlConnection(_connectionString);
                await conn.OpenAsync().ConfigureAwait(false);

                string query = "SELECT claim_id, patient_id, patient_name, " +
                               "insurance_provider, policy_number, claim_date, " +
                               "claim_amount, claim_status, notes " +
                               "FROM get_insurance_claims_by_patient_id(@patientId)";

                await using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("patientId", patientId);

                await using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);

                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    var insuranceClaim = new InsuranceClaimDTO
                    {
                        Id = reader.GetInt32(0),
                        PatientId = reader.GetInt32(1),
                        PatientName = reader.GetString(2),
                        InsuranceProvider = reader.GetString(3),
                        PolicyNumber = reader.IsDBNull(4) ? null : reader.GetString(4),
                        ClaimDate = reader.GetDateTime(5),
                        ClaimAmount = reader.GetInt32(6),
                        ClaimStatus = reader.GetString(7),
                        Notes = reader.IsDBNull(8) ? null : reader.GetString(8)
                    };

                    insuranceClaims.Add(insuranceClaim);
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                Console.WriteLine($"Database error occurred while retrieving insurance claims: {npgsqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving insurance claims: {ex.Message}");
            }

            return insuranceClaims;
        }


        public async Task<int> CreateInsuranceClaimAsync(CreateInsuranceClaimDTO insuranceClaim)
        {
            int newClaimId = 0;

            try
            {
                await using var conn = new NpgsqlConnection(_connectionString);
                await conn.OpenAsync().ConfigureAwait(false);

                string query = "SELECT create_insurance_claim( " +
                                    "@patientId::int, " +
                                    "@insuranceProvider::text, " +
                                    "@policyNumber::text," +
                                    "@claimDate::date, " +
                                    "@claimAmount::int, " +
                                    "@claimStatus::text, " +
                                    "@notes::text )";


                await using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("patientId", NpgsqlTypes.NpgsqlDbType.Integer, insuranceClaim.PatientId);
                cmd.Parameters.AddWithValue("insuranceProvider", NpgsqlTypes.NpgsqlDbType.Text, insuranceClaim.InsuranceProvider);
                cmd.Parameters.AddWithValue("policyNumber", NpgsqlTypes.NpgsqlDbType.Text, insuranceClaim.PolicyNumber ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("claimDate", NpgsqlTypes.NpgsqlDbType.Date, insuranceClaim.ClaimDate);
                cmd.Parameters.AddWithValue("claimAmount", NpgsqlTypes.NpgsqlDbType.Integer, insuranceClaim.ClaimAmount);
                cmd.Parameters.AddWithValue("claimStatus", NpgsqlTypes.NpgsqlDbType.Text, insuranceClaim.ClaimStatus);
                cmd.Parameters.AddWithValue("notes", NpgsqlTypes.NpgsqlDbType.Text, insuranceClaim.Notes ?? (object)DBNull.Value);


                newClaimId = (int)await cmd.ExecuteScalarAsync().ConfigureAwait(false);
            }
            catch (NpgsqlException npgsqlEx)
            {
                Console.WriteLine($"Database error occurred while creating insurance claim: {npgsqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while creating insurance claim: {ex.Message}");
            }

            return newClaimId;
        }



    }
}