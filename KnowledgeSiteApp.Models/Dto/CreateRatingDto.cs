namespace KnowledgeSiteApp.Models.Dto
{
    public class CreateRatingDto
    {
        public int TrainingId { get; set; }
        public int Stars { get; set; }
        public string Comment { get; set; }
    }
}
