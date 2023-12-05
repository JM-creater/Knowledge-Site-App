using AutoMapper;
using KnowledgeSiteApp.Backend.Core.Context;
using KnowledgeSiteApp.Backend.Core.Dto;
using KnowledgeSiteApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeSiteApp.Backend.Service
{
    public class TrainingCategoryService : ITrainingCategoryService
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        public TrainingCategoryService(AppDbContext dbcontext, IMapper _mapper)
        {
            context = dbcontext;
            mapper = _mapper;
        }

        public async Task<TrainingCategory> Create(TrainingCategoryCreateDto dto)
        {
            var existingCategoryName = await context.TrainingCategories
                                                    .Where(c => c.CategoryName == dto.CategoryName)
                                                    .FirstOrDefaultAsync();

            if (existingCategoryName != null) 
            {
                throw new InvalidOperationException("Category with name already exists");
            }

            var addCategory = mapper.Map<TrainingCategory>(dto);

            addCategory.IsActive = true;
            addCategory.DateCreated = DateTime.Now;

            context.TrainingCategories.Add(addCategory);
            await context.SaveChangesAsync();

            return addCategory;
        }

        public Task<List<TrainingCategory>> GetAll()
            => context.TrainingCategories
                      .Include(c => c.Trainings)
                      .OrderByDescending(c => c.DateCreated)
                      .ToListAsync();

        public async Task<TrainingCategory> GetById(int id)
            => await context.TrainingCategories
                            .Include(c => c.Trainings)
                            .Where(t => t.Id == id)
                            .FirstOrDefaultAsync();

        public async Task<IEnumerable<TrainingCategory>> SearchCategory(string search)
        {
            var category = await context.TrainingCategories
                                        .Where(c => c.CategoryName.Contains(search))
                                        .ToListAsync();

            if (string.IsNullOrWhiteSpace(search)) 
            {
                return await context.TrainingCategories.ToListAsync();
            }

            return category;
        }

        public async Task<TrainingCategory> Update(int id, TrainingCategoryUpdateDto dto)
        {
            var category = await context.TrainingCategories
                                        .Where(c => c.Id == id)
                                        .FirstOrDefaultAsync();

            if (category == null)
                throw new InvalidOperationException("Training Category not found");

            mapper.Map(dto, category);

            context.TrainingCategories.Update(category);
            await context.SaveChangesAsync();

            return category;
        }

        public async Task<TrainingCategory> Activate(int id)
        {
            var category = await context.TrainingCategories
                                        .Where(ct => ct.Id == id)
                                        .FirstOrDefaultAsync();

            if (category == null)
                throw new InvalidOperationException("Training Category not found");

            category.IsActive = true;

            context.TrainingCategories.Update(category);
            await context.SaveChangesAsync();

            return category;
        }

        public async Task<TrainingCategory> Deactivate(int id)
        {
            var category = await context.TrainingCategories
                                        .Where(ct => ct.Id == id)
                                        .FirstOrDefaultAsync();

            if (category == null)
                throw new InvalidOperationException("Training Category not found");

            category.IsActive = false;

            context.TrainingCategories.Update(category);
            await context.SaveChangesAsync();

            return category;
        }

        public async Task<TrainingCategory> Delete(int id)
        {
            var category = await context.TrainingCategories
                                        .Where(c => c.Id == id)
                                        .FirstOrDefaultAsync();

            if (category == null)
                throw new InvalidOperationException("Training Category not found");

            context.TrainingCategories.Remove(category);
            await context.SaveChangesAsync();

            return category;
        }
    }
}
