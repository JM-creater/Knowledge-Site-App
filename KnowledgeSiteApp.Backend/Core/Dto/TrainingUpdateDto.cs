namespace KnowledgeSiteApp.Backend.Core.Dto
{
    public class TrainingUpdateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile CoverImage { get; set; }
        public int CategoryId { get; set; }
    }
}
