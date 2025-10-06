namespace UserApi.Logic.Models
{
    public class UserLogic
    {
        public required Guid Id { get; init; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public required string Role { get; set; }
        public required DateTime CreatedAt { get; set; }
        public required bool IsActive { get; set; }
    }
}
