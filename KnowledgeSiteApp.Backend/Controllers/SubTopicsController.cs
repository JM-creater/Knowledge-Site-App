using KnowledgeSiteApp.Backend.Attributes;
using KnowledgeSiteApp.Backend.Core.Dto;
using KnowledgeSiteApp.Backend.Service;
using KnowledgeSiteApp.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeSiteApp.Backend.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class SubTopicsController : ControllerBase
    {
        private readonly ISubTopicService service;
        public SubTopicsController(ISubTopicService _service)
        {
            service = _service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSubTopicDto dto)
        {
            var subTopic = await service.Create(dto);

            return Ok(subTopic);
        }

        [HttpPost("SaveSubTopicResources/{id}")]
        public async Task<IActionResult> SaveSubTopicResources(int id, [FromForm] SubTopicResourcesDto dto)
        {
            var updatedTraining = await service.SaveSubTopicResources(id, dto);

            return Ok(updatedTraining);
        }

        [HttpGet]
        public async Task<IActionResult> GetSubTopic()
        {
            var subTopic = await service.GetSubTopic();

            return Ok(subTopic);
        }

        [HttpGet("{subTopicId}")]
        public async Task<IActionResult> GetByIdSubTopic([FromRoute] int subTopicId)
        {
            var subTopic = await service.GetById(subTopicId);

            return Ok(subTopic);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubTopic([FromRoute] int id, [FromBody] SubTopicUpdateDto dto)
        {
            var subTopic = await service.Update(id, dto);

            return Ok(subTopic);
        }

        [HttpPost("SaveSubTopicVideo/{id}")]
        public async Task<IActionResult> SaveSubTopicVideo(int id, [FromForm] SubTopicVideoDto dto)
        {
            var updatedTraining = await service.SaveSubTopicVideo(id, dto);

            return Ok(updatedTraining);
        }
    }
}
