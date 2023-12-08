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

        public async Task<Rating> GetById(int id)
            => await context.Ratings
                            .Where(r => r.Id == id)
                            .FirstOrDefaultAsync();
    }
}
