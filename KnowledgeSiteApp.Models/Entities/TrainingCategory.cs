using System.ComponentModel.DataAnnotations;

namespace KnowledgeSiteApp.Models.Entities;

public class TrainingCategory
{
    [Key]
    public int Id { get; set; }
    public string CategoryName { get; set; }
    public bool IsActive { get; set; }
    public DateTime DateCreated { get; set; }
}
