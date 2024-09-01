
using HospitalDataLayer.Infrastructure.DTOs.User;
using HospitalDataLayer.Infrastructure.Helpers;
using HospitalDataLayer.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace HospitalDataLayer.Infrastructure
{
    public class clsUserData : IUserData
    {
        private readonly string _connectionString;
        private readonly ILogger<clsUserData> _logger;
        private readonly PasswordHelper _passwordHelper;
        public clsUserData(string connectionString, ILogger<clsUserData> logger, PasswordHelper passwordHelper)
        {
            _connectionString = connectionString;
            _logger = logger;
            _passwordHelper = passwordHelper;
        }

        public async Task<int> CreateUserAsync(CreateUserDTO user)
        {

            int newUserId = 0;

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string query = "SELECT create_user(" +
                                   "@Username::VARCHAR, " +
                                   "@PasswordHash::VARCHAR, " +
                                   "@RoleId::INT )";

                    string hashedPassword = _passwordHelper.HashPassword(user.PasswordHash);

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {

                        cmd.Parameters.AddWithValue("Username", user.Username);
                        cmd.Parameters.AddWithValue("PasswordHash", hashedPassword);
                        cmd.Parameters.AddWithValue("RoleId", user.RoleId);

                        newUserId = (await cmd.ExecuteScalarAsync() as int?) ?? 0;
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                _logger.LogError(npgsqlEx, "Database error occurred while creating the User ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the User ");
            }

            return newUserId;
        }


        public async Task<bool> DeleteUserAsync(int userId)
        {
            bool isDeleted = false;

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string query = "SELECT delete_user_by_id(@UserId::INT)";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("UserId", userId);

                        var result = await cmd.ExecuteScalarAsync();
                        isDeleted = (result != null && (bool)result);
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                _logger.LogError(npgsqlEx, "Database error occurred while deleting the User ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the User ");
            }

            return isDeleted;
        }


        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var UsersList = new List<UserDTO>();

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {

                    await conn.OpenAsync();

                    string query = "SELECT user_id, username, " +
                                   "role, created_at, updated_at " +
                                   "FROM get_all_users()";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var user = new UserDTO
                            {
                                Id = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                Role = reader.GetString(2),
                                CreatedAt = reader.GetDateTime(3),
                                UpdatedAt = reader.GetDateTime(4),

                            };
                            UsersList.Add(user);
                        }
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                _logger.LogError(npgsqlEx, "Database error occurred while fetching users.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching users.");
            }

            return UsersList;
        }


        public async Task<UserDTO> GetUserByIdAsync(int userId)
        {
            UserDTO? user = null;

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    string query = "SELECT user_id, username, " +
                                    "role, created_at, updated_at " +
                                    "FROM get_user_by_id(@Id)";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("Id", userId);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                user = new UserDTO
                                {
                                    Id = reader.GetInt32(0),
                                    Username = reader.GetString(1),
                                    Role = reader.GetString(2),
                                    CreatedAt = reader.GetDateTime(3),
                                    UpdatedAt = reader.GetDateTime(4),
                                };
                            }
                        }
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                _logger.LogError(npgsqlEx, "Database error occurred while fetching the User by ID");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the User by ID");
            }

            return user!;
        }

        public async Task<UserDTO> GetUserByUsernameAsync(string username)
        {
            UserDTO? user = null;

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    string query = "SELECT user_id, username, " +
                                   "role, created_at, updated_at " +
                                   "FROM get_user_by_username(@Username)";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("Username", username);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                user = new UserDTO
                                {
                                    Id = reader.GetInt32(0),
                                    Username = reader.GetString(1),
                                    Role = reader.GetString(2),
                                    CreatedAt = reader.GetDateTime(3),
                                    UpdatedAt = reader.GetDateTime(4),
                                };
                            }
                        }
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                _logger.LogError(npgsqlEx, "Database error occurred while fetching the User by Username");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the User by Username");
            }

            return user!;
        }


        private async Task<string> GetPasswordHashByUsernameAsync(string username)
        {
            string passwordHash = null;

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    string query = "SELECT get_password_hash_by_username(@Username)";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("Username", username);

                        passwordHash = (string)await cmd.ExecuteScalarAsync();
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                _logger.LogError(npgsqlEx, "Database error occurred while fetching the User by Username");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the User by Username");
            }

            return passwordHash;
        }


        private async Task<bool> IsUserExistsByUsernameAsync(string username)
        {

            bool userExists = false;

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    string query = "SELECT is_user_exists_by_username(@Username)";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("Username", username);

                        userExists = (await cmd.ExecuteScalarAsync() as bool?) ?? false;
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                _logger.LogError(npgsqlEx, "Database error occurred while checking if the User exists by Username");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking if the User exists by Username");
            }

            return userExists;
        }


        public async Task<bool> IsValidCredentialsAsync(string username, string password)
        {
            bool isValid = false;

            try
            {

                bool userExists = await IsUserExistsByUsernameAsync(username);

                if (userExists)
                {
                    string passwordHash = await GetPasswordHashByUsernameAsync(username);

                    if (!string.IsNullOrEmpty(passwordHash))
                    {

                        var verificationResult = _passwordHelper.VerifyPassword(password, passwordHash);

                        isValid = verificationResult == PasswordVerificationResult.Success;
                    }
                }

            }
            catch (NpgsqlException npgsqlEx)
            {
                _logger.LogError(npgsqlEx, "Database error occurred while checking credentials");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking credentials");
            }

            return isValid;
        }

        public Task<bool> UpdatePasswordByUsernameAsync(string username, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateRoleByUsernameAsync(string username, int newRoleId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateUserAsync(int userId, UpdateUserDTO updateUserDto)
        {
            bool isUpdated = false;

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    string query = "SELECT update_user_by_id(" +
                                   "@UserId::INT, " +
                                   "@Username::VARCHAR, " +
                                   "@RoleId::INT )";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("UserId", userId);
                        cmd.Parameters.AddWithValue("Username", updateUserDto.Username);
                        cmd.Parameters.AddWithValue("RoleId", updateUserDto.RoleId);

                        var result = await cmd.ExecuteScalarAsync();
                        isUpdated = (result != null && (bool)result);
                    }
                }
            }
            catch (NpgsqlException npgsqlEx)
            {
                _logger.LogError(npgsqlEx, "Database error occurred while updating the User ");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the User ");
            }

            return isUpdated;
        }

        public Task<bool> UpdateUsernameAsync(string username, string newUsername)
        {
            throw new NotImplementedException();
        }
    }
}