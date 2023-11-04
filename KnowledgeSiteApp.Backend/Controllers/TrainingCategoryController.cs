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

        [HttpPost]
        public async Task<IActionResult> CreateCategory(string categoryName)
        {
            try
            {
                var trainingCategory = await service.Create(categoryName);

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

        [HttpPut("update/{id}/{name}")]
        public async Task<IActionResult> UpdateCategory(int id, string name)
        {
            try
            {
                var category = await service.Update(id, name);

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
