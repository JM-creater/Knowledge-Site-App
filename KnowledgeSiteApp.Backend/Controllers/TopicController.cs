using KnowledgeSiteApp.Backend.Core.Dto;
using KnowledgeSiteApp.Backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeSiteApp.Backend.Controllers
{
    [ApiController, Route("api/[controller]")]
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
            try
            {
                var topic = await service.Create(dto);

                return Ok(topic);   
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTopic()
        {
            try
            {
                var topic = await service.GetAll();

                return Ok(topic);   
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdTopic([FromRoute] int id)
        {
            try
            {
                var topic = await service.GetById(id);

                return Ok(topic);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTopic([FromRoute] int id, [FromBody] TopicUpdateDto dto)
        {
            try
            {
                var topic = await service.Update(id, dto);

                return Ok(topic);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTopic([FromRoute] int id)
        {
            try
            {
                var topic = await service.Delete(id);

                return Ok(topic);   
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


    }
}
