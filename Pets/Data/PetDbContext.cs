using Microsoft.EntityFrameworkCore;
using Pets.Models;

namespace Pets.Data
{
    public class PetDbContext : DbContext
    {
        public PetDbContext(DbContextOptions<PetDbContext> options)
            : base(options)
        {
        }

        public DbSet<Pet> Pets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ensure you have: using Microsoft.EntityFrameworkCore;
            modelBuilder.Entity<Pet>(entity =>
            {
                entity.ToTable("Pets"); // This method comes from Microsoft.EntityFrameworkCore.Relational
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
                entity.Property(p => p.AnimalType).IsRequired().HasMaxLength(50);
                entity.Property(p => p.Description).HasMaxLength(500);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
