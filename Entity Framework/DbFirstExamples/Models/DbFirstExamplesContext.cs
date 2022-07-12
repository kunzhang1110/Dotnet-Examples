using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DbFirstExamples.Models
{
    public partial class DbFirstExamplesContext : DbContext
    {
        public DbFirstExamplesContext()
        {
        }

        public DbFirstExamplesContext(DbContextOptions<DbFirstExamplesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Article> Articles { get; set; } = null!;
        public virtual DbSet<ArticleTag> ArticleTags { get; set; } = null!;
        public virtual DbSet<ArticlesWithTag> ArticlesWithTags { get; set; } = null!;
        public virtual DbSet<Tag> Tags { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=MY-LEGION;Database=DbFirstExamples;Trusted_Connection=True;Encrypt=No;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("_id");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ArticleTag>(entity =>
            {
                entity.ToTable("ArticleTag");

                entity.Property(e => e.Id).HasColumnName("_id");

                entity.Property(e => e.ArticleId).HasColumnName("ArticleID");

                entity.Property(e => e.TagId).HasColumnName("TagID");

                entity.HasOne(d => d.Article)
                    .WithMany(p => p.ArticleTags)
                    .HasForeignKey(d => d.ArticleId)
                    .HasConstraintName("FK__ArticleTa__Artic__440B1D61");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.ArticleTags)
                    .HasForeignKey(d => d.TagId)
                    .HasConstraintName("FK__ArticleTa__TagID__44FF419A");
            });

            modelBuilder.Entity<ArticlesWithTag>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("Articles_With_Tag");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
