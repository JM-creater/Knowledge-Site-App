using KnowledgeSiteApp.Backend.Core.Context;
using KnowledgeSiteApp.Backend.Core.Dto;
using KnowledgeSiteApp.Models.Entities;

namespace KnowledgeSiteApp.Backend.Service
{
    public class RatingService : IRatingService
    {
        private readonly AppDbContext context;
        public RatingService(AppDbContext dbcontext)
        {
            context = dbcontext;
        }

        public async Task<Rating> SubmitRating(RatingCreateDto dto)
        {
            try
            {
                var rating = new Rating
                {
                    TrainingId = dto.TrainingId,
                    Stars = dto.Stars,
                    Comment = dto.Comment,
                    DateCreated = DateTime.Now
                };

                context.Ratings.Add(rating);
                await context.SaveChangesAsync();

                return rating;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }
    }
}
