using AutoMapper;
using KnowledgeSiteApp.Backend.Core.Context;
using KnowledgeSiteApp.Backend.Core.Dto;
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

        public async Task<string?> coverImages(IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            string mainFolder = Path.Combine(Directory.GetCurrentDirectory(), "Images");
            string subFolder = Path.Combine(mainFolder, "CoverImage");

            if (!Directory.Exists(mainFolder))
            {
                Directory.CreateDirectory(mainFolder);
            }
            if (!Directory.Exists(subFolder))
            {
                Directory.CreateDirectory(subFolder);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(subFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return Path.Combine("Images", "CoverImage", fileName);
        }

        public async Task<Training> Create(TrainingCreateDto dto)
        {
            try
            {
                var user = await context.Users.FindAsync(dto.AdminId);
                if (user == null)
                    throw new InvalidOperationException("Admin does not exists");

                var existingTraining = await context.Trainings
                                            .Where(t => t.Title == dto.Title)
                                            .FirstOrDefaultAsync();

                if (existingTraining != null)
                    throw new InvalidOperationException("Training with the same title already exists");

                var coverImagePath = await coverImages(dto.CoverImage);

                var newTraining = mapper.Map<Training>(dto);
                newTraining.CoverImage = coverImagePath;
                newTraining.AdminId = dto.AdminId;

                context.Trainings.Add(newTraining);
                await context.SaveChangesAsync();

                return newTraining;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
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

                var newCoverImagePath = await coverImages(dto.CoverImage);

                training.Title = dto.Title;
                training.Description = dto.Description;
                training.CoverImage = newCoverImagePath;
                training.CategoryId = dto.CategoryId;

                context.Trainings.Update(training);
                await context.SaveChangesAsync();

                return training;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
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
                throw new Exception(e.Message);
            }
        }

    }
}
