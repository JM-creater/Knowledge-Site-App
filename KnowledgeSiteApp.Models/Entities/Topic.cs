using System.ComponentModel.DataAnnotations;

namespace KnowledgeSiteApp.Models.Entities
{
    public class Topic
    {
        [Key]
        public int TopicId { get; set; }
        public string Title { get; set; }
        public string Resource { get; set; }
        public int TrainingId { get; set; }
        public virtual Training Training { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
