using KnowledgeSiteApp.Backend.Attributes;
using KnowledgeSiteApp.Backend.Core.Dto;
using KnowledgeSiteApp.Backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeSiteApp.Backend.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class RatingController : Controller
    {
        private readonly IRatingService service;
        public RatingController(IRatingService _service)
        {
            service = _service;
        }

        [HttpPost]
        public async Task<IActionResult> Submit([FromBody] RatingCreateDto dto)
        {
            var rating = await service.SubmitRating(dto);

            return Ok(rating);
        }

        [HttpGet("average/{trainingId}")]
        public async Task<IActionResult> GetAverageRating(int trainingId)
        {
            var averageRating = await service.GetAverageRatingForTraining(trainingId);

            return Ok(new { AverageRating = averageRating });
        }

        [HttpGet("average-by-admin/{adminId}")]
        public async Task<IActionResult> GetAverageRatingsByAdmin(int adminId)
        {
            var averageRatings = await service.GetAverageRatingsByAdminId(adminId);

            return Ok(averageRatings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var rating = await service.GetById(id);

            return Ok(rating);
        }
    }
}
