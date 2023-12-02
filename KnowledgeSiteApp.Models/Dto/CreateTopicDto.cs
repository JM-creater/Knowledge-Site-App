using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSiteApp.Models.Dto
{
    public class CreateTopicDto
    {
        public string Title { get; set; }
        public string YouTubeUrl { get; set; }
        public string Description { get; set; }
        public int TrainingId { get; set; }
    }
}
