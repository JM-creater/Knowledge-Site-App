using System.ComponentModel.DataAnnotations;

namespace KnowledgeSiteApp.Models.Entities;

public class SubTopic
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Video { get; set; }
    public string Description { get; set; }
    public string YouTubeUrl { get; set; }
    public string? Resource { get; set; }
    public int? TopicId { get; set; }
    public virtual Topic Topic { get; set; }
    public DateTime DateCreated { get; set; }
}
