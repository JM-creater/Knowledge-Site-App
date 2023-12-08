using AutoMapper;
using KnowledgeSiteApp.Backend.Core.Context;
using KnowledgeSiteApp.Backend.Core.Dto;
using KnowledgeSiteApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeSiteApp.Backend.Service
{
    public class RatingService : IRatingService
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        public RatingService(AppDbContext dbcontext, IMapper _mapper)
        {
            context = dbcontext;
            mapper = _mapper;
        }

        public async Task<Rating> SubmitRating(RatingCreateDto dto)
        {
            var addRating = mapper.Map<Rating>(dto);
            addRating.Comment = dto.Comment;
            addRating.Stars = dto.Stars;
            addRating.DateCreated = DateTime.Now;

            context.Ratings.Add(addRating);
            await context.SaveChangesAsync();

            return addRating;
        }

        public async Task<double> GetAverageRatingForTraining(int trainingId)
        {
            var ratings = await context.Ratings
                                       .Where(r => r.TrainingId == trainingId)
                                       .ToListAsync();

            if (!ratings.Any())
            {
                return 0; 
            }

            var sumOfRatings = ratings.Sum(r => r.Stars);
            var numberOfRatings = ratings.Count() * 5;

            return (double)sumOfRatings / numberOfRatings;
        }

        public async Task<Dictionary<int, double>> GetAverageRatingsByAdminId(int adminId)
        {
            var trainings = await context.Trainings
                                         .Where(t => t.AdminId == adminId)
                                         .Include(t => t.Ratings)
                                         .ToListAsync();

            var averageRatings = new Dictionary<int, double>();
            foreach (var training in trainings)
            {
                if (training.Ratings.Any())
                {
                    var averageRating = training.Ratings.Average(r => r.Stars);
                    averageRatings.Add(training.Id, averageRating);
                }
                else
                {
                    averageRatings.Add(training.Id, 0);
                }
            }

            return averageRatings;
        }

        public async Task<Rating> GetById(int id)
            => await context.Ratings
                            .Where(r => r.Id == id)
                            .FirstOrDefaultAsync();
    }
}
