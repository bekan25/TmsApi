namespace TmsApi.Entities;

public class Enrollment
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public int CourseId { get; set; }

    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;

    public decimal? Grade { get; set; }

    // Navigation property to Student
    public Student Student { get; set; } = null!;

    // Navigation property to Course
    public Course Course { get; set; } = null!;
}