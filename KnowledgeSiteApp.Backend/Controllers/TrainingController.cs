using KnowledgeSiteApp.Backend.Attributes;
using KnowledgeSiteApp.Backend.Core.Dto;
using KnowledgeSiteApp.Backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeSiteApp.Backend.Controllers
{
    [ApiController, Route("api/[controller]")]
    [ApiKey]
    public class TrainingController : ControllerBase
    {
        private readonly ITrainingService service;
        public TrainingController(ITrainingService _service)
        {
            service = _service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTraining([FromBody] TrainingCreateDto dto)
        {
            var training = await service.Create(dto);

            return Ok(training);
        }

        [HttpPost("SaveTrainingImage/{id}")]
        public async Task<IActionResult> SaveTrainingImage(int id, [FromForm] ImageCreateDto dto)
        {
            var updatedTraining = await service.SaveTrainingImage(id, dto);

            return Ok(updatedTraining);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTraining()
        {
            var training = await service.GetAll();

            return Ok(training);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdTraining([FromRoute] int id)
        {
            var trainingCategory = await service.GetById(id);

            return Ok(trainingCategory);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTraining(int id, [FromForm] TrainingUpdateDto dto)
        {
            var training = await service.Update(id, dto);

            return Ok(training);
        }

        [HttpPut("activate/{id}")]
        public async Task<IActionResult> Activation(int id)
        {
            var training = await service.Activate(id);

            return Ok(training);
        }

        [HttpPut("deactivate/{id}")]
        public async Task<IActionResult> Deactivation(int id)
        {
            var training = await service.Deactivate(id);

            return Ok(training);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteByIdTraining(int id)
        {
            var training = await service.Delete(id);

            return Ok(training);
        }
    }
}
