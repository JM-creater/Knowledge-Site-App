﻿using KnowledgeSiteApp.Backend.Attributes;
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
        [ApiKey]
        public async Task<IActionResult> Create(CreateSubTopicDto dto)
        {
            var subTopic = await service.Create(dto);

            return Ok(subTopic);
        }

        [HttpGet]
        [ApiKey]
        public async Task<IActionResult> GetSubTopic()
        {
            var subTopic = await service.GetSubTopic();

            return Ok(subTopic);
        }

        [HttpPost("SaveSubTopicResources/{id}")]
        public async Task<IActionResult> SaveSubTopicResources(int id, [FromForm] SubTopicResourcesDto dto)
        {
            var updatedTraining = await service.SaveSubTopicResources(id, dto);

            return Ok(updatedTraining);
        }

        [HttpPost("SaveSubTopicVideo/{id}")]
        public async Task<IActionResult> SaveSubTopicVideo(int id, [FromForm] SubTopicVideoDto dto)
        {
            var updatedTraining = await service.SaveSubTopicVideo(id, dto);

            return Ok(updatedTraining);
        }
    }
}
