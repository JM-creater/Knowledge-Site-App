using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
