using Microsoft.EntityFrameworkCore;
using TmsApi;
using TmsApi.Entities;

var builder = WebApplication.CreateBuilder(args);

// Register TmsDbContext with PostgreSQL and enable SQL logging
builder.Services.AddDbContext<TmsDbContext>(options =>
options
.UseNpgsql(
builder.Configuration.GetConnectionString("TmsDatabase"))
.LogTo(Console.WriteLine, LogLevel.Information)
.EnableSensitiveDataLogging()
);

// Register Controllers
builder.Services.AddControllers();

// Register Enrollment Service
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();

// Register OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure OpenAPI in Development environment
if (app.Environment.IsDevelopment())
{
app.MapOpenApi();
}

// HTTPS Redirection
app.UseHttpsRedirection();

// Map API Controllers
app.MapControllers();
// Seed test data at application startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TmsDbContext>();

    // Apply any pending migrations
    context.Database.Migrate();

    // Seed only if Students table is empty
    if (!context.Students.Any())
    {
        var students = new List<Student>
        {
            new()
            {
                RegistrationNumber = "TMS-2026-0001",
                Name = "Alice Smith",
                GPA = 3.8m,
                IsActive = true
            },
            new()
            {
                RegistrationNumber = "TMS-2026-0002",
                Name = "Bob Jones",
                GPA = 2.9m,
                IsActive = true
            },
            new()
            {
                RegistrationNumber = "TMS-2026-0003",
                Name = "Charlie Brown",
                GPA = 3.4m,
                IsActive = false
            },
            new()
            {
                RegistrationNumber = "TMS-2026-0004",
                Name = "Diana Prince",
                GPA = 3.9m,
                IsActive = true
            },
            new()
            {
                RegistrationNumber = "TMS-2026-0005",
                Name = "Evan Wright",
                GPA = 2.5m,
                IsActive = true
            }
        };

        context.Students.AddRange(students);

        var courses = new List<Course>
        {
            new()
            {
                Code = "CS-101",
                Title = "Introduction to Computer Science",
                Capacity = 30
            },
            new()
            {
                Code = "CS-201",
                Title = "Data Structures and Algorithms",
                Capacity = 25
            },
            new()
            {
                Code = "MAT-101",
                Title = "Calculus I",
                Capacity = 40
            }
        };

        context.Courses.AddRange(courses);

        // Save Students and Courses first
        context.SaveChanges();

        var enrollments = new List<Enrollment>
        {
            new()
            {
                StudentId = students[0].Id,
                CourseId = courses[0].Id,
                Grade = 4.0m
            },
            new()
            {
                StudentId = students[0].Id,
                CourseId = courses[1].Id,
                Grade = 3.6m
            },
            new()
            {
                StudentId = students[1].Id,
                CourseId = courses[0].Id,
                Grade = 2.8m
            },
            new()
            {
                StudentId = students[3].Id,
                CourseId = courses[1].Id,
                Grade = 3.9m
            }
        };

        context.Enrollments.AddRange(enrollments);

        context.SaveChanges();
    }
}

// Run Application
app.Run();
