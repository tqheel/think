using Microsoft.EntityFrameworkCore;
using ThinkDiary.Core.Models;

namespace ThinkDiary.Data;

public class DiaryDbContext : DbContext
{
    public DiaryDbContext(DbContextOptions<DiaryDbContext> options) : base(options)
    {
    }

    public DbSet<DiaryEntry> DiaryEntries { get; set; }
    public DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure DiaryEntry
        modelBuilder.Entity<DiaryEntry>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Content).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired();
            
            // Configure many-to-many relationship with Tags
            entity.HasMany(e => e.Tags)
                  .WithMany(t => t.Entries)
                  .UsingEntity("DiaryEntryTags");
        });

        // Configure Tag
        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Name).IsRequired().HasMaxLength(100);
            entity.Property(t => t.Color).HasMaxLength(7); // For hex color codes
            entity.HasIndex(t => t.Name).IsUnique();
        });
    }
}
