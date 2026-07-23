using Microsoft.EntityFrameworkCore;
using TmsApi.Entities;

namespace TmsApi;


public class TmsDbContext : DbContext
{

    public TmsDbContext(DbContextOptions<TmsDbContext> options)
        : base(options)
    {

    }

    public DbSet<Assessment> Assessments { get; set; }

    public DbSet<Certificate> Certificates { get; set; }
    public DbSet<Student> Students { get; set; }

    public DbSet<Course> Courses { get; set; }

    public DbSet<Enrollment> Enrollments { get; set; }

}

