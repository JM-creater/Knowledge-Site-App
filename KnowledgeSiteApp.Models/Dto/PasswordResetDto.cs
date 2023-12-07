namespace KnowledgeSiteApp.Models.Dto
{
    public class PasswordResetDto
    {
        public string? Token { get; set; }
        public string? NewPassword { get; set; }
    }
}
