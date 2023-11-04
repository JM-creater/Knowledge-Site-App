using KnowledgeSiteApp.Backend.Core.Enum;

namespace KnowledgeSiteApp.Backend.Core.Dto
{
    public class RegisterUserDto
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public IFormFile? ProfileImage { get; set; }
        public string WebsiteUrl { get; set; }
        public string ArticlesUrl { get; set; }
        public IFormFile? BooksImages { get; set; }
    }
}
