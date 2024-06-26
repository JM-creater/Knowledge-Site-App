﻿using AutoMapper;
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
            var existingTraining = await context.Trainings
                                                .Where(t => t.Title == dto.Title)
                                                .FirstOrDefaultAsync();

            if (existingTraining != null)
                throw new InvalidOperationException("Training with the same title already exists");

            var newTraining = mapper.Map<Training>(dto);
            newTraining.IsActive = true;
            newTraining.DateCreated = DateTime.Now;

            context.Trainings.Add(newTraining);
            await context.SaveChangesAsync();

            return newTraining;
        }

        public async Task<Training> SaveTrainingImage(int id, ImageCreateDto dto)
        {
            var training = await context.Trainings.FindAsync(id);

            if (training == null)
                throw new InvalidOperationException("Training not found");

            var uploadTrainingImage = await new ImagePathConfig().trainingImages(dto.image);

            training.Image = uploadTrainingImage;

            await context.SaveChangesAsync();

            return training;
        }

        public async Task<IEnumerable<Training>> GetAllTrainingByCategory(int? categoryId)
        {
            var trainings = context.Trainings.AsQueryable();

            if (categoryId.HasValue)
            {
                trainings = trainings.Where(t => t.CategoryId == categoryId);
            }

            return await trainings.ToListAsync();
        }

        public async Task<List<Training>> GetAll()
            => await context.Trainings
                            .Include(t => t.Admin)
                            .Include(t => t.Topics)
                                .ThenInclude(t => t.SubTopics)
                            .ToListAsync();

        public async Task<Training> GetById(int id)
            => await context.Trainings
                            .Include(t => t.Admin)
                            .Include(t => t.Category)
                            .Include(t => t.Topics)
                                .ThenInclude(t => t.SubTopics)
                            .Include(t => t.Ratings)
                            .Where(u => u.Id == id)
                            .FirstOrDefaultAsync();

        public async Task<List<Training>> GetByTrainingIdForUser(int id)
            => await context.Trainings
                            .Include(t => t.Category)
                            .Include(t => t.Topics)
                                .ThenInclude(t => t.SubTopics)
                            .Where(u => u.Id == id)
                            .ToListAsync();

        public async Task<List<Training>> GetTrainingByAdminId(int adminId)
        {
            return await context.Trainings
                                .Include(t => t.Admin)
                                .Include(t => t.Topics)
                                    .ThenInclude(t => t.SubTopics)
                                .Where(t => t.AdminId == adminId)
                                .ToListAsync();
        }

        public async Task<IEnumerable<Training>> SearchTraining(string search)
        {
            var training = await context.Trainings
                                        .Where(t => t.Title.Contains(search))
                                        .ToListAsync();

            if (string.IsNullOrWhiteSpace(search))
            {
                return await context.Trainings.ToListAsync();
            }

            return training;
        }

        public async Task<Training> Update(int id, TrainingUpdateDto dto)
        {
            var training = await context.Trainings
                                        .Where(t => t.Id == id)
                                        .FirstOrDefaultAsync();

            if (training == null)
                throw new InvalidOperationException("Training does not exist");

            training.Title = dto.Title;
            training.Description = dto.Description;
            training.CategoryId = dto.CategoryId;

            context.Trainings.Update(training);
            await context.SaveChangesAsync();

            return training;
        }

        public async Task<Training> Activate(int id)
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

        public async Task<Training> Deactivate(int id)
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

        public async Task<Training> Delete(int id)
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

    }
}
