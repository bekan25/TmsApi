using Microsoft.EntityFrameworkCore;
using TmsApi.Entities;

namespace TmsApi;


public interface IEnrollmentService
{
    Task<Enrollment?> GetByIdAsync(int id);

    Task<List<Enrollment>> GetAllAsync();

    Task<Enrollment> EnrollAsync(int studentId, int courseId);

    Task<bool> DeleteAsync(int id);
}



public class EnrollmentService : IEnrollmentService
{
    private readonly TmsDbContext _context;
    private readonly ILogger<EnrollmentService> _logger;


    public EnrollmentService(
        TmsDbContext context,
        ILogger<EnrollmentService> logger)
    {
        _context = context;
        _logger = logger;
    }



    public async Task<Enrollment?> GetByIdAsync(int id)
    {
        return await _context.Enrollments
            .Include(e => e.Student)
            .Include(e => e.Course)
            .FirstOrDefaultAsync(e => e.Id == id);
    }



    public async Task<List<Enrollment>> GetAllAsync()
    {
        return await _context.Enrollments
            .Include(e => e.Student)
            .Include(e => e.Course)
            .ToListAsync();
    }



    public async Task<Enrollment> EnrollAsync(
        int studentId,
        int courseId)
    {

        var enrollment = new Enrollment
        {
            StudentId = studentId,
            CourseId = courseId,
            EnrolledAt = DateTime.UtcNow
        };


        _context.Enrollments.Add(enrollment);

        await _context.SaveChangesAsync();


        _logger.LogInformation(
            "Student {StudentId} enrolled to Course {CourseId}",
            studentId,
            courseId);


        return enrollment;
    }



    public async Task<bool> DeleteAsync(int id)
    {
        var enrollment = await _context.Enrollments
            .FindAsync(id);


        if (enrollment == null)
        {
            return false;
        }


        _context.Enrollments.Remove(enrollment);

        await _context.SaveChangesAsync();


        return true;
    }
}