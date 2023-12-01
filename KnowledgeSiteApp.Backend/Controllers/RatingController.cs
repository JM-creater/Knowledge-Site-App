using KnowledgeSiteApp.Backend.Attributes;
using KnowledgeSiteApp.Backend.Core.Dto;
using KnowledgeSiteApp.Backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace KnowledgeSiteApp.Backend.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService service;
        public RatingController(IRatingService _service)
        {
            service = _service;
        }

        [HttpPost]
        public async Task<IActionResult> Submit(RatingCreateDto dto)
        {
            var rating = await service.SubmitRating(dto);

            return Ok(rating);
        }
    }
}
