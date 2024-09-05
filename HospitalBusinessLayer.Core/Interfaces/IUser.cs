using HospitalDataLayer.Infrastructure.DTOs.User;

namespace HospitalBusinessLayer.Core.Interfaces
{
    public interface IUser
    {
        Task<int> CreateUserAsync(CreateUserDTO user);
        Task<bool> DeleteUserAsync(int userId);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUserByIdAsync(int userId);
        Task<UserDTO> GetUserByUsernameAsync(string username);
        Task<bool> UpdateUserAsync(int userId, UpdateUserDTO updateUserDto);
        Task<bool> IsValidCredentialsAsync(string username, string password);
    }
}