using System.ComponentModel.DataAnnotations;

namespace AP.BusinessInterfaces.Data.User;

public class UserSignup
{
    [Key] public Guid Id { get; init; }
    public required string Email { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}