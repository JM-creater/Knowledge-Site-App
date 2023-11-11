using KnowledgeSiteApp.Backend.Core.Dto;
using KnowledgeSiteApp.Backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeSiteApp.Backend.Controllers
{
    [ApiController, Route("api/[controller]")]
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
            try
            {
                var trainingCategory = await service.Create(dto);

                return Ok(trainingCategory);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetByAllCategory()
        {
            try
            {
                var category = await service.GetAll();

                return Ok(category);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdCategory(int id)
        {
            try
            {
                var category = await service.GetById(id);

                return Ok(category);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string searchTerm)
        {
            try
            {
                var categories = await service.SearchCategory(searchTerm);
                return Ok(categories);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, TrainingCategoryUpdateDto dto)
        {
            try
            {
                var category = await service.Update(id, dto);

                return Ok(category);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("activate/{id}")]
        public async Task<IActionResult> Activation(int id)
        {
            try
            {
                var category = await service.Activate(id);

                return Ok(category);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("deactivate/{id}")]
        public async Task<IActionResult> Deactivation(int id)
        {
            try
            {
                var category = await service.Deactivate(id);

                return Ok(category);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var category = await service.Delete(id);

                return Ok(category);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
