﻿using Microsoft.EntityFrameworkCore;
using MobileAppAPI.DBModels.Accounts;
using MobileAppAPI.DBModels.Chat;
using MobileAppAPI.DBModels.Content;

namespace MobileAppAPI.DBModels
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<PostModel> Posts { get; set; }
        public DbSet<RefreshTokenModel> RefreshTokens { get; set; }
        public DbSet<ChatSessionModel> ChatSessions { get; set; }
        public DbSet<ParticipantModel> ChatParticipants { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ParticipantModel>()
                .HasKey(p => new { p.ChatId, p.UserId });
        }
    }
}