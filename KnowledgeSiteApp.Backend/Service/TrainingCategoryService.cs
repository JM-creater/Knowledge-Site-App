using KnowledgeSiteApp.Backend.Core.Context;
using KnowledgeSiteApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeSiteApp.Backend.Service
{
    public class TrainingCategoryService : ITrainingCategoryService
    {
        private readonly AppDbContext context;
        public TrainingCategoryService(AppDbContext dbcontext)
        {
            context = dbcontext;
        }

        public async Task<TrainingCategory> Create(string categoryName)
        {
            try
            {
                var newTrainingCategory = new TrainingCategory()
                {
                    CategoryName = categoryName
                };

                context.TrainingCategories.Add(newTrainingCategory);
                await context.SaveChangesAsync();

                return newTrainingCategory;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Task<List<TrainingCategory>> GetAll()
            => context.TrainingCategories.ToListAsync();

        public async Task<List<TrainingCategory>> GetById(int id)
            => await context.TrainingCategories
                            .Where(t => t.Id == id)
                            .ToListAsync();

        public async Task<TrainingCategory> Update(int id, string name)
        {
            try
            {
                var category = await context.TrainingCategories
                                            .Where(c => c.Id == id)
                                            .FirstOrDefaultAsync();

                if (category == null)
                    throw new InvalidOperationException("Training Category not found");

                category.CategoryName = name;

                context.TrainingCategories.Update(category);

                await context.SaveChangesAsync();

                return category;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<TrainingCategory> Delete(int id)
        {
            try
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
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }
    }
}
