﻿using KnowledgeSiteApp.Backend.Core.Encryption;
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

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "Admin123",
                    FirstName = "Admin",
                    LastName = "Admin",
                    Email = "admin0123@gmail.com",
                    Password = PasswordHasher.EncryptPassword("123456"),
                    Role = 2,
                    IsActive = true,
                    DateCreated = DateTime.Now
                }
           );

            modelBuilder.Entity<Training>()
                .HasMany(t => t.Ratings)
                .WithOne(r => r.Training)
                .HasForeignKey(r => r.TrainingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Training) 
                .WithMany(t => t.Ratings) 
                .HasForeignKey(r => r.TrainingId) 
                .OnDelete(DeleteBehavior.Cascade);

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

            modelBuilder.Entity<TrainingCategory>()
                .HasOne(c => c.Admin)
                .WithMany() 
                .HasForeignKey(c => c.AdminId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
