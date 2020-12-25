using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace SimpleStackOverflow.Data
{
    public class QuestionsContext : DbContext
    {
        private string _connectionString;

        public QuestionsContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        public DbSet<Question> Questions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<QuestionTag> QuestionsTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<QuestionTag>()
                .HasKey(qt => new { qt.QuestionId, qt.TagId });

            modelBuilder.Entity<QuestionTag>()
                .HasOne(qt => qt.Question)
                .WithMany(q => q.QuestionTags)
                .HasForeignKey(q => q.QuestionId);

            modelBuilder.Entity<QuestionTag>()
                .HasOne(qt => qt.Tag)
                .WithMany(t => t.QuestionTags)
                .HasForeignKey(q => q.TagId);

            modelBuilder.Entity<Like>()
                .HasKey(l => new {l.UserId, l.QuestionId });

            modelBuilder.Entity<Like>()
                .HasOne(l => l.Question)
                .WithMany(q => q.Likes)
                .HasForeignKey(q => q.QuestionId);

            modelBuilder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(u => u.UserId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
