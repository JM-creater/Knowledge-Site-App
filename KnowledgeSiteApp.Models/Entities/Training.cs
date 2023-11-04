using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSiteApp.Models.Entities;

public class Training
{
    [Key]
    public int Id { get; set; }
    public string? Title { get; set; }
    public int AdminId { get; set; }
    public User Admin { get; set; }
    public int RatingId { get; set; }
    public virtual Rating Rating { get; set; }
    public string Description { get; set; }
    public string CoverImage { get; set; }
    public int CategoryId { get; set; }
    public virtual TrainingCategory Category { get; set; }
    public ICollection<Topic> Topics { get; set; } = new List<Topic>();
}
