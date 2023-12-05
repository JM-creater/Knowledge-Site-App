using KnowledgeSiteApp.Backend.Attributes;
using KnowledgeSiteApp.Backend.Core.Dto;
using KnowledgeSiteApp.Backend.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeSiteApp.Backend.Controllers
{
    [ApiController, Route("api/[controller]")]
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

        [HttpGet("trainingByCategory")]
        public async Task<IActionResult> GetTrainingsByCategory(int? categoryId)
        {
            var training = await service.GetAllTrainingByCategory(categoryId);

            return Ok(training);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdTraining([FromRoute] int id)
        {
            var trainingCategory = await service.GetById(id);

            return Ok(trainingCategory);
        }

        [HttpGet("trainingByAdminId/{adminId}")]
        public async Task<IActionResult> GetTrainingByAdminId([FromRoute] int adminId)
        {
            var trainingCategory = await service.GetTrainingByAdminId(adminId);

            return Ok(trainingCategory);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTraining(int id, [FromBody] TrainingUpdateDto dto)
        {
            var training = await service.Update(id, dto);

            return Ok(training);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string searchTerm)
        {
            var categories = await service.SearchTraining(searchTerm);

            return Ok(categories);
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
