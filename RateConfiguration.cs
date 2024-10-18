using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace cmcs_.Models
{
    public class RateConfiguration : IEntityTypeConfiguration<Rate>
    {
        public void Configure(EntityTypeBuilder<Rate> builder)
        {
            builder.HasKey(r => r.Id); // Set the primary key
            builder.Property(r => r.LecturerName)
                .IsRequired()
                .HasMaxLength(100); // Set max length and required

            builder.Property(r => r.HourlyRate)
                .IsRequired(); // Ensure hourly rate is required
        }
    }
}
