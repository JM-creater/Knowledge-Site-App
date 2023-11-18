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
            try
            {
                var addCategory = mapper.Map<TrainingCategory>(dto);

                addCategory.DateCreated = DateTime.Now;

                context.TrainingCategories.Add(addCategory);
                await context.SaveChangesAsync();

                return addCategory;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public Task<List<TrainingCategory>> GetAll()
            => context.TrainingCategories
                      .OrderByDescending(c => c.DateCreated)
                      .ToListAsync();

        public async Task<List<TrainingCategory>> GetById(int id)
            => await context.TrainingCategories
                            .Where(t => t.Id == id)
                            .ToListAsync();

        public async Task<IEnumerable<TrainingCategory>> SearchCategory(string search)
        {
            try
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
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<TrainingCategory> Update(int id, TrainingCategoryUpdateDto dto)
        {
            try
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
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<TrainingCategory> Activate(int id)
        {
            try
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
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<TrainingCategory> Deactivate(int id)
        {
            try
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
