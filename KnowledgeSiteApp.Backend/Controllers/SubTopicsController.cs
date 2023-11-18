using KnowledgeSiteApp.Backend.Attributes;
using KnowledgeSiteApp.Backend.Core.Dto;
using KnowledgeSiteApp.Backend.Service;
using KnowledgeSiteApp.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeSiteApp.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubTopicsController : ControllerBase
    {
        private readonly ISubTopicService service;
        public SubTopicsController(ISubTopicService _service)
        {
            this.service = _service;
        }

        [HttpPost]
        [ApiKey]
        public async Task<IActionResult> Create(CreateSubTopicDto dto)
        {
            try
            {
                var subTopic = await service.Create(dto);
                return Ok(subTopic);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        [HttpPost("SaveSubTopicResources/{id}")]
        [ApiKey]
        public async Task<IActionResult> SaveSubTopicResources(int id, [FromForm] SubTopicResourcesDto dto)
        {
            try
            {
                var updatedTraining = await service.SaveSubTopicResources(id, dto);
                return Ok(updatedTraining);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("SaveSubTopicVideo/{id}")]
        [ApiKey]
        public async Task<IActionResult> SaveSubTopicVideo(int id, [FromForm] SubTopicVideoDto dto)
        {
            try
            {
                var updatedTraining = await service.SaveSubTopicVideo(id, dto);
                return Ok(updatedTraining);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
