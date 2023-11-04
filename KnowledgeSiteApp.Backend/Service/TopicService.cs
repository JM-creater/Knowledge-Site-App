using AutoMapper;
using KnowledgeSiteApp.Backend.Core.Context;
using KnowledgeSiteApp.Backend.Core.Dto;
using KnowledgeSiteApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeSiteApp.Backend.Service
{
    public class TopicService : ITopicService
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        public TopicService(AppDbContext dbcontext, IMapper _mapper)
        {
            context = dbcontext;
            mapper = _mapper;
        }

        public async Task<Topic> Create(TopicCreateDto dto)
        {
            try
            {
                var topic = await context.Topics
                                         .Where(t => t.Title == dto.Title)
                                         .FirstOrDefaultAsync();

                if (topic != null)
                    throw new InvalidOperationException("Topic is already exists");

                var newTopic = mapper.Map<Topic>(dto);

                context.Topics.Add(newTopic);
                await context.SaveChangesAsync();

                return newTopic;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<List<Topic>> GetAll()
            => await context.Topics
                            .Include(t => t.Training)
                                .ThenInclude(t => t.Admin)
                            .Include(t => t.Training)
                                .ThenInclude(t => t.Category)
                            .ToListAsync();

        public async Task<List<Topic>> GetById(int id)
            => await context.Topics
                            .Include(t => t.Training)
                                .ThenInclude(t => t.Admin)
                            .Include(t => t.Training)
                                .ThenInclude(t => t.Category)
                            .Where(t => t.TopicId == id)
                            .ToListAsync();

        public async Task<Topic> Update(int id, TopicUpdateDto dto)
        {
            try
            {
                var topic = await context.Topics
                                         .Where(t => t.TopicId == id)
                                         .FirstOrDefaultAsync();

                if (topic == null)
                    throw new InvalidOperationException("Topic not found");

                mapper.Map(dto, topic);
                topic.Title = dto.Title;
                topic.Resource = dto.Resource;
                topic.TrainingId = dto.TrainingId;

                await context.SaveChangesAsync();

                return topic;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<Topic> Delete(int id)
        {
            var topic = await context.Topics
                                        .Where(t => t.TopicId == id)
                                        .FirstOrDefaultAsync();

            if (topic == null)
                throw new InvalidOperationException("Topic not found");

            context.Topics.Remove(topic);
            await context.SaveChangesAsync();   

            return topic;
        }
    }
}
