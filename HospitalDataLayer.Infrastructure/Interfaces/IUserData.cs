using HospitalDataLayer.Infrastructure.DTOs.User;

namespace HospitalDataLayer.Infrastructure.Interfaces
{
    public interface IUserData
    {
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUserByIdAsync(int userId);
        Task<UserDTO> GetUserByUsernameAsync(string username);
        Task<int> CreateUserAsync(CreateUserDTO user);
        Task<bool> UpdateUserAsync(int userId, UpdateUserDTO updateUserDto);
        Task<bool> DeleteUserAsync(int userId);
        Task<bool> UpdatePasswordByUsernameAsync(string username, string newPassword);
        Task<bool> UpdateUsernameAsync(string username, string newUsername);
        Task<bool> UpdateRoleByUsernameAsync(string username, int newRoleId);
        Task<bool> IsValidCredentialsAsync(string username, string password);

    }
}