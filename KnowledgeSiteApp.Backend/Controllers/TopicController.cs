using KnowledgeSiteApp.Backend.Attributes;
using KnowledgeSiteApp.Backend.Core.Dto;
using KnowledgeSiteApp.Backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeSiteApp.Backend.Controllers
{
    [ApiController, Route("api/[controller]")]
    [ApiKey]
    public class TopicController : ControllerBase
    {
        private readonly ITopicService service;
        public TopicController(ITopicService _service)
        {
            service = _service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTopic([FromBody] TopicCreateDto dto)
        {
            var topic = await service.Create(dto);

            return Ok(topic);   
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTopic()
        {
            var topic = await service.GetAll();

            return Ok(topic);   
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdTopic([FromRoute] int id)
        {
            var topic = await service.GetById(id);

            return Ok(topic);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTopic([FromRoute] int id, [FromBody] TopicUpdateDto dto)
        {
            var topic = await service.Update(id, dto);

            return Ok(topic);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTopic([FromRoute] int id)
        {
            var topic = await service.Delete(id);

            return Ok(topic);   
        }
    }
}
