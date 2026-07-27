using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TmsApi.Entities;

namespace TmsApi.Configurations;

public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        // Primary key
        builder.HasKey(e => e.Id);

        // Enrollment date is required
        builder.Property(e => e.EnrolledAt)
            .IsRequired();

        // Grade is optional
        builder.Property(e => e.Grade)
            .HasPrecision(5, 2);

        // A student can have many enrollments.
        // Restrict prevents deleting a student while enrollment records exist.
        builder.HasOne(e => e.Student)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        // A course can have many enrollments.
        // Restrict prevents deleting a course while enrollment records exist.
        builder.HasOne(e => e.Course)
            .WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}