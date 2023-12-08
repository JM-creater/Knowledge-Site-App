using KnowledgeSiteApp.Backend.Core.Dto;
using KnowledgeSiteApp.Models.Entities;

namespace KnowledgeSiteApp.Backend.Service
{
    public interface IRatingService
    {
        public Task<Rating> SubmitRating(RatingCreateDto dto);
        public Task<Dictionary<int, double>> GetAverageRatingsByAdminId(int adminId);
        public Task<double> GetAverageRatingForTraining(int trainingId);
        public Task<Rating> GetById(int id);
    }
}
