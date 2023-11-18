using KnowledgeSiteApp.Backend.Attributes;
using KnowledgeSiteApp.Backend.Core.Dto;
using KnowledgeSiteApp.Backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeSiteApp.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiKey]
    public class TrainingCategoryController : ControllerBase
    {
        private readonly ITrainingCategoryService service;
        public TrainingCategoryController(ITrainingCategoryService _service)
        {
            service = _service;
        }

        [HttpPost("add")]
        public async Task<IActionResult> CreateCategory(TrainingCategoryCreateDto dto)
        {
            var trainingCategory = await service.Create(dto);

            return Ok(trainingCategory);
        }

        [HttpGet]
        public async Task<IActionResult> GetByAllCategory()
        {
            var category = await service.GetAll();

            return Ok(category);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdCategory(int id)
        {
            var category = await service.GetById(id);

            return Ok(category);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string searchTerm)
        {
            var categories = await service.SearchCategory(searchTerm);

            return Ok(categories);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, TrainingCategoryUpdateDto dto)
        {
            var category = await service.Update(id, dto);

            return Ok(category);
        }

        [HttpPut("activate/{id}")]
        public async Task<IActionResult> Activation(int id)
        {
            var category = await service.Activate(id);

            return Ok(category);
        }

        [HttpPut("deactivate/{id}")]
        public async Task<IActionResult> Deactivation(int id)
        {
            var category = await service.Deactivate(id);

            return Ok(category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await service.Delete(id);

            return Ok(category);
        }
    }
}
