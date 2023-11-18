﻿using AutoMapper;
using KnowledgeSiteApp.Backend.Core.Context;
using KnowledgeSiteApp.Backend.Core.Dto;
using KnowledgeSiteApp.Models.Entities;
using Microsoft.EntityFrameworkCore;
using KnowledgeSiteApp.Models;
using KnowledgeSiteApp.Models.Dto;
using KnowledgeSiteApp.Backend.Core.ImageDirectory;

namespace KnowledgeSiteApp.Backend.Service
{
    public class SubTopicService : ISubTopicService
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        public SubTopicService(AppDbContext dbcontext, IMapper imapper)
        {
            this.context = dbcontext;
            this.mapper = imapper;
        }

        public async Task<SubTopic> Create(CreateSubTopicDto dto)
        {
            try
            {
                var subTopic = await context.SubTopics
                                         .Where(t => t.Title == dto.Title)
                                         .FirstOrDefaultAsync();

                if (subTopic != null)
                    throw new InvalidOperationException("Topic is already exists");

                var newSubTopic = mapper.Map<SubTopic>(dto);

                newSubTopic.DateCreated = DateTime.Now;

                context.SubTopics.Add(newSubTopic);
                await context.SaveChangesAsync();

                return newSubTopic;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<SubTopic> SaveSubTopicResources(int id, SubTopicResourcesDto dto)
        {
            try
            {
                var subTopic = await context.SubTopics.FindAsync(id);

                if (subTopic == null)
                    throw new InvalidOperationException("Sub Topic not found");

                var uploadTrainingImage = await new ImagePathConfig().resourceImages(dto.Resource);

                subTopic.Resource = uploadTrainingImage;

                await context.SaveChangesAsync();

                return subTopic;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }
        public async Task<SubTopic> SaveSubTopicVideo(int id, SubTopicVideoDto dto)
        {
            try
            {
                var subTopic = await context.SubTopics.FindAsync(id);

                if (subTopic == null)
                    throw new InvalidOperationException("Sub Topic not found");

                var uploadTrainingImage = await new ImagePathConfig().subTopicVideo(dto.Video);

                subTopic.Video = uploadTrainingImage;

                await context.SaveChangesAsync();

                return subTopic;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }
    }
}