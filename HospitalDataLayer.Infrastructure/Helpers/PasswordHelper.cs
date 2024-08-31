using HospitalDataLayer.Infrastructure.DTOs.User;
using Microsoft.AspNetCore.Identity;

namespace HospitalDataLayer.Infrastructure.Helpers
{
    public class PasswordHelper
    {
        private readonly PasswordHasher<CreateUserDTO> _passwordHasher = new PasswordHasher<CreateUserDTO>();

        public string HashPassword(CreateUserDTO user, string password)
        {
            return _passwordHasher.HashPassword(user, password);
        }

        public PasswordVerificationResult VerifyPassword(CreateUserDTO user, string password, string hashedPassword)
        {
            return _passwordHasher.VerifyHashedPassword(user, hashedPassword, password);
        }
    }
}