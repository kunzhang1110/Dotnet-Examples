using Microsoft.EntityFrameworkCore;


namespace EFCoreExamples.Models
{
    public partial class EFCoreExamplesContext : DbContext
    {
        private readonly string _connectionString = "Server=MY-LEGION;Database=EFCoreExamples;Trusted_Connection=True;Encrypt=false";

        public EFCoreExamplesContext()
        {
            //the empty constructor is needed for design time DbContext Creation
        }

        //Tables
        public virtual DbSet<Article> Articles { get; set; } = null!;
        public virtual DbSet<ArticleTag> ArticleTags { get; set; } = null!;
        public virtual DbSet<Tag> Tags { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>(entity =>
            {
                entity.ToTable("Articles"); //optional
                entity.Property(e => e.Id).HasColumnName("_id");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });


            modelBuilder.Entity<Tag>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("_id");

                entity.Property(e => e.Name)
                    .HasMaxLength(225)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ArticleTag>(entity =>
            {
                entity.ToTable("ArticleTag");

                entity.Property(e => e.Id).HasColumnName("_id");

                entity.Property(e => e.ArticleId).HasColumnName("ArticleID");

                entity.Property(e => e.TagId).HasColumnName("TagID");

                entity.HasOne(articleTag => articleTag.Article)
                    .WithMany(article => article.ArticleTags)
                    .HasForeignKey(articleTag => articleTag.ArticleId)
                    .HasConstraintName("FK__ArticleTa__Artic__5629CD9C");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.ArticleTags)
                    .HasForeignKey(d => d.TagId)
                    .HasConstraintName("FK__ArticleTa__TagID__571DF1D5");
            });
        }
    }
}
