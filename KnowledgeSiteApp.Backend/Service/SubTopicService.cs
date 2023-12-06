using AutoMapper;
using KnowledgeSiteApp.Backend.Core.Context;
using KnowledgeSiteApp.Backend.Core.Dto;
using KnowledgeSiteApp.Models.Entities;
using Microsoft.EntityFrameworkCore;
using KnowledgeSiteApp.Models.Dto;
using KnowledgeSiteApp.Backend.Core.ImageDirectory;

namespace KnowledgeSiteApp.Backend.Service
{
    public class SubTopicService : ISubTopicService
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        public SubTopicService(AppDbContext dbcontext, IMapper _mapper)
        {
            context = dbcontext;
            mapper = _mapper;
        }

        public async Task<SubTopic> Create(CreateSubTopicDto dto)
        {
            var subTopic = await context.SubTopics
                                        .Where(t => t.Title == dto.Title)
                                        .FirstOrDefaultAsync();

            if (subTopic != null)
                throw new InvalidOperationException("Sub-Topic is already exists");

            var newSubTopic = mapper.Map<SubTopic>(dto);

            newSubTopic.DateCreated = DateTime.Now;

            context.SubTopics.Add(newSubTopic);
            await context.SaveChangesAsync();

            return newSubTopic;
        }

        public async Task<List<SubTopic>> GetSubTopic()
            => await context.SubTopics
                            .Include(st => st.Topic)
                            .ToListAsync();

        public async Task<SubTopic> GetById(int subTopicId)
            => await context.SubTopics
                            .Include(st => st.Topic)
                            .Where(st => st.Id == subTopicId)
                            .FirstOrDefaultAsync();

        public async Task<SubTopic> Update(int id, SubTopicUpdateDto dto)
        {
            var subTopic = await context.SubTopics
                                        .Where(t => t.Id == id)
                                        .FirstOrDefaultAsync();

            if (subTopic == null)
                throw new InvalidOperationException("Topic not found");

            mapper.Map(dto, subTopic);
            subTopic.Title = dto.Title;
            subTopic.Description = dto.Description;
            subTopic.YouTubeUrl = dto.YouTubeUrl;

            context.SubTopics.Update(subTopic);
            await context.SaveChangesAsync();

            return subTopic;
        }

        public async Task<SubTopic> Activate(int id)
        {
            var subTopic = await context.SubTopics
                                     .Where(t => t.Id == id)
                                     .FirstOrDefaultAsync();

            if (subTopic == null)
                throw new InvalidOperationException("Sub Topic not found");

            subTopic.IsActive = true;

            context.SubTopics.Update(subTopic);
            await context.SaveChangesAsync();

            return subTopic;
        }

        public async Task<SubTopic> Deactivate(int id)
        {
            var subTopic = await context.SubTopics
                                     .Where(t => t.Id == id)
                                     .FirstOrDefaultAsync();

            if (subTopic == null)
                throw new InvalidOperationException("Topic not found");

            subTopic.IsActive = false;

            context.SubTopics.Update(subTopic);
            await context.SaveChangesAsync();

            return subTopic;
        }

        public async Task<SubTopic> SaveSubTopicResources(int id, SubTopicResourcesDto dto)
        {
            var subTopic = await context.SubTopics.FindAsync(id);

            if (subTopic == null)
                throw new InvalidOperationException("Sub Topic not found");

            var uploadTrainingImage = await new ImagePathConfig().subTopicResource(dto.Resource);

            subTopic.Resource = uploadTrainingImage;

            await context.SaveChangesAsync();

            return subTopic;
        }
        public async Task<SubTopic> SaveSubTopicVideo(int id, SubTopicVideoDto dto)
        {
            var subTopic = await context.SubTopics.FindAsync(id);

            if (subTopic == null)
                throw new InvalidOperationException("Sub Topic not found");

            var uploadTrainingImage = await new ImagePathConfig().subTopicVideo(dto.Video);

            subTopic.Video = uploadTrainingImage;

            await context.SaveChangesAsync();

            return subTopic;
        }
    }
}
