using KnowledgeSiteApp.Backend.Core.Dto;
using KnowledgeSiteApp.Models.Entities;

namespace KnowledgeSiteApp.Backend.Service
{
    public interface IRatingService
    {
        public Task<Rating> SubmitRating(RatingCreateDto dto);
    }
}
