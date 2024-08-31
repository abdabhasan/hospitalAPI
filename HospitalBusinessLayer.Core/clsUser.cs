
using HospitalDataLayer.Infrastructure.DTOs.User;
using HospitalDataLayer.Infrastructure.Interfaces;

namespace HospitalBusinessLayer.Core
{
    public class clsUser
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

        public async Task<bool> UpdateUserAsync(int userId, UpdateUserDTO updateUserDto)
        {
            return await _userData.UpdateUserAsync(userId, updateUserDto);
        }


    }
}