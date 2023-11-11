using KnowledgeSiteApp.Backend.Core.Dto;
using KnowledgeSiteApp.Models.Entities;

namespace KnowledgeSiteApp.Backend.Service
{
    public interface ITrainingCategoryService
    {
        public Task<TrainingCategory> Create(TrainingCategoryCreateDto dto);
        public Task<List<TrainingCategory>> GetById(int id);
        public Task<List<TrainingCategory>> GetAll();
        public Task<IEnumerable<TrainingCategory>> SearchCategory(string search);
        public Task<TrainingCategory> Update(int id, TrainingCategoryUpdateDto dto);
        public Task<TrainingCategory> Delete(int id);
        public Task<TrainingCategory> Activate(int id);
        public Task<TrainingCategory> Deactivate(int id);
    }
}
