namespace HospitalDataLayer.Infrastructure.DTOs.User
{
    public class CreateUserDTO
    {
        public string Username { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public int RoleId { get; set; }
    }
}