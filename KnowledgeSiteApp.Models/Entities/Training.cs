using System.ComponentModel.DataAnnotations;

namespace KnowledgeSiteApp.Models.Entities;

public class Training
{
    [Key]
    public int Id { get; set; }
    public string? Title { get; set; }
    public int? AdminId { get; set; }
    public User Admin { get; set; }
    public int? RatingId { get; set; }
    public virtual Rating Rating { get; set; }
    public string Description { get; set; }
    public string? Image { get; set; }
    public int CategoryId { get; set; }
    public virtual TrainingCategory Category { get; set; }
    public bool IsActive { get; set; }
    public DateTime DateCreated { get; set; }
    public ICollection<Topic> Topics { get; set; } = new List<Topic>();
}