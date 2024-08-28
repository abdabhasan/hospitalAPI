using HospitalDataLayer.Infrastructure.DTOs.Bill;
using HospitalDataLayer.Infrastructure.Interfaces;
using Npgsql;

namespace HospitalDataLayer.Infrastructure
{
    public class clsBillData : IBillData
    {

        private readonly string _connectionString;
        public clsBillData(string connectionString)
        {
            _connectionString = connectionString;
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

                        newBillId = (int)await cmd.ExecuteScalarAsync();
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                Console.WriteLine($"Database error occurred while creating the bill: {npgsqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while creating the bill: {ex.Message}");
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
                Console.WriteLine($"Database error occurred while deleting the bill: {npgsqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting the bill: {ex.Message}");
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
                Console.WriteLine($"Database error occurred while retrieving bills: {npgsqlEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving bills: {ex.Message}");
            }

            return bills;
        }

        public Task<BillDTO> GetBillByIdAsync(int billId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BillDTO>> GetBillsForPatientByPatientIdAsync(int patientId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BillDTO>> GetBillsForPatientByPatientNameAsync(string patientName)
        {
            throw new NotImplementedException();
        }
    }
}