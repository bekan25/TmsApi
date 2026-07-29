using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TmsApi.Entities;

namespace TmsApi.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        // Table configuration
        builder.ToTable("Courses");

        // Primary key
        builder.HasKey(c => c.Id);

        // Name
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        // Code
        builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(20);

        // Description
        builder.Property(c => c.Description)
            .HasMaxLength(500);

        // Credit Hours
        builder.Property(c => c.CreditHours)
            .IsRequired();

        // Unique Course Code
        builder.HasIndex(c => c.Code)
            .IsUnique();

        // Course has many Enrollments
        builder.HasMany(c => c.Enrollments)
            .WithOne(e => e.Course)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}