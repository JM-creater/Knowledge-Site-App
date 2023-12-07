using System.ComponentModel.DataAnnotations;

namespace KnowledgeSiteApp.Models.Entities;

public class TrainingCategory
{
    [Key]
    public int Id { get; set; }
    public int AdminId { get; set; }
    public virtual User Admin { get; set; }
    public string CategoryName { get; set; }
    public bool IsActive { get; set; }
    public DateTime DateCreated { get; set; }

    public virtual ICollection<Training> Trainings { get; set; } = new List<Training>();
}
