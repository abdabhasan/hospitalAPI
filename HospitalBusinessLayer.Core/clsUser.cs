
using HospitalBusinessLayer.Core.Interfaces;
using HospitalDataLayer.Infrastructure.DTOs.User;
using HospitalDataLayer.Infrastructure.Interfaces;

namespace HospitalBusinessLayer.Core
{
    public class clsUser : IUser
    {
        private readonly IUserData _userData;

        public clsUser(IUserData userData)
        {
            _userData = userData;
        }


        public async Task<int> CreateUserAsync(CreateUserDTO user)
        {
            return await _userData.CreateUserAsync(user);
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            return await _userData.DeleteUserAsync(userId);
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            return await _userData.GetAllUsersAsync();
        }

        public async Task<UserDTO> GetUserByIdAsync(int userId)
        {
            return await _userData.GetUserByIdAsync(userId);
        }

        public async Task<UserDTO> GetUserByUsernameAsync(string username)
        {
            return await _userData.GetUserByUsernameAsync(username);
        }

        public async Task<bool> UpdateUserAsync(int userId, UpdateUserDTO updateUserDto)
        {
            return await _userData.UpdateUserAsync(userId, updateUserDto);
        }

        public async Task<bool> IsValidCredentialsAsync(string username, string password)
        {
            return await _userData.IsValidCredentialsAsync(username, password);
        }

    }
}