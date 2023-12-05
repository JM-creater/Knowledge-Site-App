namespace KnowledgeSiteApp.Models.Dto
{
    public class TrainingCreateDto
    {
        public string Title { get; set; }
        public int AdminId { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
    }
}
