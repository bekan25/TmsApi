using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TmsApi.Entities;

namespace TmsApi.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        // Primary key
        builder.HasKey(c => c.Id);

        // Course code is required and unique
        builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(c => c.Code)
            .IsUnique();

        // Course title is required
        builder.Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(200);

        // Course description is optional
        builder.Property(c => c.Description)
            .HasMaxLength(1000);

        // Course capacity
        builder.Property(c => c.Capacity)
            .IsRequired();
    }
}