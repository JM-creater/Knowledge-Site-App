using KnowledgeSiteApp.Backend.Core.Dto;
using KnowledgeSiteApp.Models.Entities;

namespace KnowledgeSiteApp.Backend.Service
{
    public interface ITrainingService
    {
        public Task<Training> Create(TrainingCreateDto dto);
        public Task<Training> SaveTrainingImage(int id, ImageCreateDto dto);
        public Task<IEnumerable<Training>> GetAllTrainingByCategory(int? categoryId);
        public Task<List<Training>> GetAll();
        public Task<Training> GetById(int id);
        public Task<List<Training>> GetByTrainingIdForUser(int id);
        public Task<List<Training>> GetTrainingByAdminId(int adminId);
        public Task<IEnumerable<Training>> SearchTraining(string search);
        public Task<Training> Update(int id, TrainingUpdateDto dto);
        public Task<Training> Activate(int id);
        public Task<Training> Deactivate(int id);
        public Task<Training> Delete(int id);
    }
}
