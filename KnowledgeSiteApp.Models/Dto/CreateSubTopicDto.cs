using KnowledgeSiteApp.Models.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeSiteApp.Models.Dto
{
    public class CreateSubTopicDto
    {
        public string Title { get; set; }
        public string YouTubeUrl { get; set; }
        public string Description { get; set; }
    }
}
