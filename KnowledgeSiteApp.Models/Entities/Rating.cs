using System.ComponentModel.DataAnnotations;

namespace KnowledgeSiteApp.Models.Entities;

public class Rating
{
    [Key]
    public int Id { get; set; }
    public int TrainingId { get; set; }
    public virtual Training Training { get; set; }
    public int Stars { get; set; }
    public string Comment { get; set; }
    public DateTime DateCreated { get; set; }
}
