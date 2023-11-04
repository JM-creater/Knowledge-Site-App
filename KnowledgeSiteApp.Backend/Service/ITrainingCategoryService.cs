using KnowledgeSiteApp.Models.Entities;

namespace KnowledgeSiteApp.Backend.Service
{
    public interface ITrainingCategoryService
    {
        public Task<TrainingCategory> Create(string categoryName);
        public Task<List<TrainingCategory>> GetById(int id);
        public Task<List<TrainingCategory>> GetAll();
        public Task<TrainingCategory> Update(int id, string name);
        public Task<TrainingCategory> Delete(int id);
    }
}
