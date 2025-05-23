namespace Remcom.Api.Models
{
    public enum UserRole
    {
        Officer,
        Admin
    }

    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public UserRole Role { get; set; }
        public bool IsLocked { get; set; }
        public int FailedLoginAttempts { get; set; }
    }
}
