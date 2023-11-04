using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSiteApp.Models.Entities;

public class TrainingCategory
{
    [Key]
    public int Id { get; set; }
    public string CategoryName { get; set; }
}
