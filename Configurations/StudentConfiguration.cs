using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TmsApi.Entities;

namespace TmsApi.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        // Primary key
        builder.HasKey(s => s.Id);

        // Registration number is required and unique
        builder.Property(s => s.RegistrationNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(s => s.RegistrationNumber)
            .IsUnique();

        // Student name is required
        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(200);

        // GPA precision
        builder.Property(s => s.GPA)
            .HasPrecision(3, 2);

        // Active status default value
        builder.Property(s => s.IsActive)
            .HasDefaultValue(true);
    }
}