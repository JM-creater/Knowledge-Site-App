using KnowledgeSiteApp.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeSiteApp.Backend.Core.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<TrainingCategory> TrainingCategories { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<SubTopic> SubTopics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<User>().HasData(
            //    new User
            //    {
            //        UserId = 1,
            //        FirstName = "Admin",
            //        LastName = "Admin",
            //        Email = "admin0123@gmail.com",
            //        Role = 3,
            //        IsActive = true,
            //        IsValidate = true
            //    }
            //    );

            modelBuilder.Entity<Training>()
                .HasOne(t => t.Rating)
                .WithMany() 
                .HasForeignKey(t => t.RatingId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Rating>()
                .HasOne(t => t.Training)
                .WithMany()
                .HasForeignKey(t => t.TrainingId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Training>()
                .HasMany(t => t.Topics)
                .WithOne(t => t.Training)
                .HasForeignKey(t => t.TrainingId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Topic>()
                .HasMany(t => t.SubTopics)
                .WithOne(s => s.Topic)
                .HasForeignKey(s => s.TopicId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
