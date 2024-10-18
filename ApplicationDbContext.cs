using Microsoft.EntityFrameworkCore;

namespace cmcs_.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets for your entities
        public DbSet<Claim> Claims { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<ClaimDocument> ClaimDocuments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure entity relationships and properties here

            // Convert Status property in Claim to string
            modelBuilder.Entity<Claim>()
                .Property(c => c.Status)
                .HasConversion<string>();

            // Define the relationship between Claim and ClaimDocument
            modelBuilder.Entity<ClaimDocument>()
                .HasOne(cd => cd.Claim)           // ClaimDocument has one Claim
                .WithMany(c => c.Documents)       // Claim has many ClaimDocuments
                .HasForeignKey(cd => cd.ClaimId)  // ClaimId in ClaimDocument is a foreign key
                .OnDelete(DeleteBehavior.Cascade); // Optional: Cascade delete when a claim is deleted
        }
    }
}
