using KnowledgeSiteApp.Backend.Core.Dto;
using KnowledgeSiteApp.Models.Entities;

namespace KnowledgeSiteApp.Backend.Service
{
    public interface ITrainingService
    {
        public Task<Training> Create(TrainingCreateDto dto);
        public Task<List<Training>> GetAll();
        public Task<List<Training>> GetById(int id);
        public Task<Training> Update(int id, TrainingUpdateDto dto);
        public Task<Training> Delete(int id);
    }
}
