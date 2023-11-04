namespace KnowledgeSiteApp.Backend.Core.Dto
{
    public class GetByAuthorDto
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string WebsiteUrl { get; set; }
        public string ArticlesUrl { get; set; }
        public string BooksImages { get; set; }
    }
}
