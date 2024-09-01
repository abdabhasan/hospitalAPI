using Microsoft.AspNetCore.Identity;

namespace HospitalDataLayer.Infrastructure.Helpers
{
    public class PasswordHelper
    {
        private readonly PasswordHasher<string> _passwordHasher = new PasswordHasher<string>();
        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(null, password);
        }

        public PasswordVerificationResult VerifyPassword(string password, string hashedPassword)
        {
            return _passwordHasher.VerifyHashedPassword(null, hashedPassword, password);
        }
    }
}