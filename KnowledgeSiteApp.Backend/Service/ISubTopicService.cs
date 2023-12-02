﻿using KnowledgeSiteApp.Backend.Core.Dto;
using KnowledgeSiteApp.Models.Dto;
using KnowledgeSiteApp.Models.Entities;

namespace KnowledgeSiteApp.Backend.Service
{
    public interface ISubTopicService 
    {
        public Task<SubTopic> Create(CreateSubTopicDto dto);
        public Task<List<SubTopic>> GetSubTopic();
        public Task<List<SubTopic>> GetById(int id);
        public Task<SubTopic> SaveSubTopicResources(int id, SubTopicResourcesDto dto);
        public Task<SubTopic> SaveSubTopicVideo(int id, SubTopicVideoDto dto);
    }
}
