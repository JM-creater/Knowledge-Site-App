namespace KnowledgeSiteApp.Backend.Core.Dto
{
    public class RegisterUserDto
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        //public IFormFile? Image { get; set; }
    }
}
