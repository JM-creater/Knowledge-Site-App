using AutoMapper;
using KnowledgeSiteApp.Backend.Core.Context;
using KnowledgeSiteApp.Backend.Core.Dto;
using KnowledgeSiteApp.Backend.Core.ImageDirectory;
using KnowledgeSiteApp.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeSiteApp.Backend.Service
{
    public class TrainingService : ITrainingService
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        public TrainingService(AppDbContext dbcontext, IMapper _mapper)
        {
            context = dbcontext;
            mapper = _mapper;
        }

        public async Task<Training> Create(TrainingCreateDto dto)
        {
            try
            {
                var existingTraining = await context.Trainings
                                                    .Where(t => t.Title == dto.Title)
                                                    .FirstOrDefaultAsync();

                if (existingTraining != null)
                    throw new InvalidOperationException("Training with the same title already exists");

                var newTraining = mapper.Map<Training>(dto);
                //newTraining.AdminId = dto.AdminId;
                newTraining.IsActive = true;
                newTraining.DateCreated = DateTime.Now;

                context.Trainings.Add(newTraining);
                await context.SaveChangesAsync();

                return newTraining;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<Training> SaveTrainingImage(int id, ImageCreateDto dto)
        {
            try
            {
                var training = await context.Trainings.FindAsync(id);

                if (training == null)
                    throw new InvalidOperationException("Training not found");

                var uploadTrainingImage = await new ImagePathConfig().trainingImages(dto.image);

                training.Image = uploadTrainingImage;

                await context.SaveChangesAsync();

                return training;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<List<Training>> GetAll()
            => await context.Trainings
                            .Include(t => t.Admin)
                            .Include(t => t.Category)         
                            .ToListAsync();

        public async Task<List<Training>> GetById(int id)
            => await context.Trainings
                            .Include(u => u.Admin)
                            .Include(u => u.Category)
                            .Where(u => u.Id == id)
                            .ToListAsync();

        public async Task<Training> Update(int id, TrainingUpdateDto dto)
        {
            try
            {
                var training = await context.Trainings
                                            .Where(t => t.Id == id)
                                            .FirstOrDefaultAsync();

                if (training == null)
                    throw new InvalidOperationException("Training does not exist");

                var newCoverImagePath = await new ImagePathConfig().trainingImages(dto.Image);

                training.Title = dto.Title;
                training.Description = dto.Description;
                training.Image = newCoverImagePath;
                training.CategoryId = dto.CategoryId;

                context.Trainings.Update(training);
                await context.SaveChangesAsync();

                return training;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<Training> Activate(int id)
        {
            try
            {
                var training = await context.Trainings
                                            .Where(t => t.Id == id)
                                            .FirstOrDefaultAsync();

                if (training == null)
                    throw new InvalidOperationException("Training not found");

                training.IsActive = true;

                context.Trainings.Update(training);
                await context.SaveChangesAsync();

                return training;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<Training> Deactivate(int id)
        {
            try
            {
                var training = await context.Trainings
                                            .Where(t => t.Id == id)
                                            .FirstOrDefaultAsync();

                if (training == null)
                    throw new InvalidOperationException("Training not found");

                training.IsActive = false;

                context.Trainings.Update(training);
                await context.SaveChangesAsync();

                return training;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<Training> Delete(int id)
        {
            try
            {
                var training = await context.Trainings
                                            .Where(t => t.Id == id)
                                            .FirstOrDefaultAsync();

                if (training == null)
                    throw new InvalidOperationException("Training not found");

                context.Trainings.Remove(training);
                await context.SaveChangesAsync();

                return training;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

    }
}
