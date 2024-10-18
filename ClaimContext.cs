using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace cmcs_.Models
{
    public class ClaimContext : DbContext
    {
        public DbSet<Claim> Claims { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<ClaimDocument> ClaimDocuments { get; set; }
        public DbSet<Rate> Rates { get; set; }

        public ClaimContext(DbContextOptions<ClaimContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply configurations for entities
            modelBuilder.ApplyConfiguration(new Configurations.PersonConfiguration());
            modelBuilder.ApplyConfiguration(new ClaimConfiguration()); 
            modelBuilder.ApplyConfiguration(new RateConfiguration()); 

            // Seed data for rates
            modelBuilder.Entity<Rate>().HasData(
                new Rate { Id = 1, LecturerName = "John Doe", HourlyRate = 300.00 },
                new Rate { Id = 2, LecturerName = "Harry Brodersen", HourlyRate = 250.00 }
            );

            // Additional entity configurations
            modelBuilder.Entity<Claim>()
                .Property(c => c.Status)
                .HasConversion<string>();

            // Consider adding indexes here if necessary
            // modelBuilder.Entity<Claim>().HasIndex(c => c.LecturerName);

            base.OnModelCreating(modelBuilder);
        }
    }
}
