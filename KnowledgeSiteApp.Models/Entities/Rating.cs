using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
