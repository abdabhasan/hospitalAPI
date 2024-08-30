using HospitalDataLayer.Infrastructure.DTOs.Bill;
using HospitalDataLayer.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace HospitalDataLayer.Infrastructure
{
    public class clsBillData : IBillData
    {

        private readonly string _connectionString;
        private readonly ILogger<clsBillData> _logger;
        public clsBillData(string connectionString, ILogger<clsBillData> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }


        public async Task<int> CreateBillAsync(CreateBillDTO bill)
        {
            int newBillId = 0;

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string query = "SELECT create_bill(" +
                                   "@PatientId::INT, " +
                                   "@Amount::INT, " +
                                   "@DateIssued::DATE, " +
                                   "@DueDate::DATE, " +
                                   "@PaymentStatus::TEXT, " +
                                   "@Notes::TEXT, " +
                                   "@PaymentMethod::TEXT)";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("PatientId", bill.PatinetId);
                        cmd.Parameters.AddWithValue("Amount", bill.Amount);
                        cmd.Parameters.AddWithValue("DateIssued", bill.DateIssued);
                        cmd.Parameters.AddWithValue("DueDate", bill.DueDate ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("PaymentStatus", bill.PaymentStatus);
                        cmd.Parameters.AddWithValue("Notes", bill.Notes);
                        cmd.Parameters.AddWithValue("PaymentMethod", bill.PaymentMethod);

                        newBillId = (await cmd.ExecuteScalarAsync() as int?) ?? 0;
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                _logger.LogError(npgsqlEx, "Database error occurred while creating the bill ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the bill ");
            }

            return newBillId;
        }


        public async Task<bool> DeleteBillAsyncById(int billId)
        {
            bool isDeleted = false;

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string query = "DELETE FROM bills WHERE bill_id = @BillId";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("BillId", billId);

                        int affectedRows = await cmd.ExecuteNonQueryAsync();
                        isDeleted = affectedRows > 0;
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                _logger.LogError(npgsqlEx, "Database error occurred while deleting the bill ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the bill ");
            }

            return isDeleted;
        }

        public async Task<IEnumerable<BillDTO>> GetAllBillsAsync()
        {
            var bills = new List<BillDTO>();

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string query = "SELECT bill_id, " +
                                    "patient_id, " +
                                    "amount, " +
                                    "date_issued, " +
                                    "due_date, " +
                                    "payment_status, " +
                                    "notes, " +
                                    "payment_method " +
                                    "FROM get_all_bills()";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var bill = new BillDTO
                                {
                                    Id = reader.GetInt32(0),
                                    PatinetId = reader.GetInt32(1),
                                    Amount = reader.GetInt32(2),
                                    DateIssued = reader.GetDateTime(3),
                                    DueDate = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4),
                                    PaymentStatus = reader.GetString(5),
                                    Notes = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                                    PaymentMethod = reader.GetString(7)
                                };

                                bills.Add(bill);
                            }
                        }
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                _logger.LogError(npgsqlEx, "Database error occurred while retrieving bills ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving bills ");
            }

            return bills;
        }

        public async Task<BillDTO> GetBillByIdAsync(int billId)
        {
            BillDTO? bill = null;

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string query = "SELECT bill_id, " +
                                    "patient_id, " +
                                    "amount, " +
                                    "date_issued, " +
                                    "due_date, " +
                                    "payment_status, " +
                                    "notes, " +
                                    "payment_method " +
                                    "FROM  get_bill_by_id(@BillId)";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("BillId", billId);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                bill = new BillDTO
                                {
                                    Id = reader.GetInt32(0),
                                    PatinetId = reader.GetInt32(1),
                                    Amount = reader.GetInt32(2),
                                    DateIssued = reader.GetDateTime(3),
                                    DueDate = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4),
                                    PaymentStatus = reader.GetString(5),
                                    Notes = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                                    PaymentMethod = reader.GetString(7)
                                };
                            }
                        }
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                _logger.LogError(npgsqlEx, "Database error occurred while retrieving the bill ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the bill ");
            }

            return bill!;
        }

        public async Task<IEnumerable<BillDTO>> GetBillsForPatientByPatientIdAsync(int patientId)
        {
            var bills = new List<BillDTO>();

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string query = "SELECT bill_id, " +
                                    "patient_id, " +
                                    "amount, " +
                                    "date_issued, " +
                                    "due_date, " +
                                    "payment_status, " +
                                    "notes, " +
                                    "payment_method " +
                                    "FROM  get_bills_for_patient_by_patient_id(@PatientId)";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("PatientId", patientId);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var bill = new BillDTO
                                {
                                    Id = reader.GetInt32(0),
                                    PatinetId = reader.GetInt32(1),
                                    Amount = reader.GetInt32(2),
                                    DateIssued = reader.GetDateTime(3),
                                    DueDate = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4),
                                    PaymentStatus = reader.GetString(5),
                                    Notes = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                                    PaymentMethod = reader.GetString(7)
                                };

                                bills.Add(bill);
                            }
                        }
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                _logger.LogError(npgsqlEx, "Database error occurred while retrieving bills ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving bills ");
            }

            return bills;
        }

        public async Task<IEnumerable<BillDTO>> GetBillsForPatientByPatientNameAsync(string patientName)
        {
            var bills = new List<BillDTO>();

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string query = "SELECT bill_id, " +
                                    "patient_id, " +
                                    "amount, " +
                                    "date_issued, " +
                                    "due_date, " +
                                    "payment_status, " +
                                    "notes, " +
                                    "payment_method " +
                                    "FROM  get_bills_for_patient_by_name(@PatientName)";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("PatientName", patientName);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var bill = new BillDTO
                                {
                                    Id = reader.GetInt32(0),
                                    PatinetId = reader.GetInt32(1),
                                    Amount = reader.GetInt32(2),
                                    DateIssued = reader.GetDateTime(3),
                                    DueDate = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4),
                                    PaymentStatus = reader.GetString(5),
                                    Notes = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                                    PaymentMethod = reader.GetString(7)
                                };

                                bills.Add(bill);
                            }
                        }
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                _logger.LogError(npgsqlEx, "Database error occurred while retrieving bills ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving bills ");
            }

            return bills;
        }
    }
}