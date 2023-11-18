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
            try
            {
                var training = await service.Create(dto);

                return Ok(training);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("SaveTrainingImage/{id}")]
        public async Task<IActionResult> SaveTrainingImage(int id, [FromForm] ImageCreateDto dto)
        {
            try
            {
                var updatedTraining = await service.SaveTrainingImage(id, dto);
                return Ok(updatedTraining);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTraining()
        {
            try
            {
                var training = await service.GetAll();

                return Ok(training);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message); 
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdTraining([FromRoute] int id)
        {
            try
            {
                var trainingCategory = await service.GetById(id);

                return Ok(trainingCategory);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTraining(int id, [FromForm] TrainingUpdateDto dto)
        {
            try
            {
                var training = await service.Update(id, dto);

                return Ok(training);
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
                var training = await service.Activate(id);

                return Ok(training);
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
                var training = await service.Deactivate(id);

                return Ok(training);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteByIdTraining(int id)
        {
            try
            {
                var training = await service.Delete(id);

                return Ok(training);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
