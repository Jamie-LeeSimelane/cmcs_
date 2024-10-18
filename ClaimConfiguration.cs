using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace cmcs_.Models
{
    public class ClaimConfiguration : IEntityTypeConfiguration<Claim>
    {
        public void Configure(EntityTypeBuilder<Claim> builder)
        {
            builder.HasKey(c => c.Id); // Set the primary key
            builder.Property(c => c.LecturerName)
                .IsRequired()
                .HasMaxLength(100); // Set max length and required

            builder.Property(c => c.Description)
                .IsRequired()
                .HasMaxLength(500); // Set max length and required

            builder.Property(c => c.Amount)
                .IsRequired(); // Make sure it's required

            builder.Property(c => c.Status)
                .IsRequired()
                .HasMaxLength(50); // Set max length

            builder.Property(c => c.DateSubmitted)
                .IsRequired(); // Set date submitted as required

            builder.Property(c => c.HoursWorked)
                .IsRequired(); // Ensure hours worked is required

            builder.Property(c => c.HourlyRate)
                .IsRequired(); // Ensure hourly rate is required

            builder.Property(c => c.Notes)
                .HasMaxLength(1000); // Optional notes, set max length

            builder.HasMany(c => c.Documents) // Assuming you have a relationship set up
                .WithOne() // Adjust as necessary based on your model relationships
                .OnDelete(DeleteBehavior.Cascade); // Set cascade delete if needed
        }
    }
}
