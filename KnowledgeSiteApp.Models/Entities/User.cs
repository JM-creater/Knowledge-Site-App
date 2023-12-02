using System.ComponentModel.DataAnnotations;

namespace KnowledgeSiteApp.Models.Entities;

public class User
{
    [Key]
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public int Role { get; set; }
    public bool IsActive { get; set; }
    public string? Image { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateUpdated { get; set; }
    public string? PasswordResetToken { get; set; }
    public DateTime? ResetTokenExpires { get; set; }
}
