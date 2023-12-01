﻿using KnowledgeSiteApp.Backend.Core.Dto;
using KnowledgeSiteApp.Models.Dto;
using KnowledgeSiteApp.Models.Entities;

namespace KnowledgeSiteApp.Backend.Service
{
    public interface ITopicService
    {
        public Task<Topic> Create(CreateTopicDto dto);
        public Task<List<Topic>> GetAll();
        public Task<List<Topic>> GetById(int id);
        public Task<Topic> Update(int id, TopicUpdateDto dto);
        public Task<Topic> Delete(int id);
        public Task<Topic> SaveTopicResources(int id, TopicResources dto);
        public Task<Topic> SaveTopicVideo(int id, TopicVideo dto);
    }
}
