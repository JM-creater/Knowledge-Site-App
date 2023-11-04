namespace KnowledgeSiteApp.Backend.Core.Dto
{
    public class RatingCreateDto
    {
        public int TrainingId { get; set; }
        public int Stars { get; set; }
        public string Comment { get; set; }
    }
}
