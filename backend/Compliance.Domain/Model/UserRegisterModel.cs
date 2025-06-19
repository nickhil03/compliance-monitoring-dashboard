namespace Compliance.Domain.Model
{
    public class UserRegisterModel
    {
        public required string Username { get; set; }
        public required string Name { get; set; }
        public string? Email { get; set; }
        public required string Password { get; set; }
        public string? Roles { get; set; }
    }
}
