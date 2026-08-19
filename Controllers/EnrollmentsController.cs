using Microsoft.AspNetCore.Mvc;
using TmsApi.Dtos;
using TmsApi.Services;

namespace TmsApi.Controllers;

[ApiController]
[Route("api/courses/{courseId:int}/enrollments")]
public class EnrollmentsController(
    ICourseService courseService,
    IEnrollmentService enrollmentService) : ControllerBase
{


    // GET: /api/courses/1/enrollments
    [HttpGet(Name = "ListCourseEnrollments")]
    public async Task<IActionResult> GetEnrollments(
        int courseId,
        CancellationToken ct)
    {
        // Check parent course exists
        var course = await courseService.GetByIdAsync(
            courseId,
            ct);


        if (course is null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Course not found",
                Status = StatusCodes.Status404NotFound
            });
        }


        var result =
            await enrollmentService.GetByCourseAsync(
                courseId,
                ct);


        return Ok(result);
    }




    // GET: /api/courses/1/enrollments/5
    [HttpGet("{id:int}", Name = nameof(GetEnrollment))]
    public async Task<IActionResult> GetEnrollment(
        int courseId,
        int id,
        CancellationToken ct)
    {
        var enrollment =
            await enrollmentService.GetByIdAsync(
                courseId,
                id,
                ct);


        if (enrollment is null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Enrollment not found",
                Status = StatusCodes.Status404NotFound
            });
        }


        return Ok(enrollment);
    }




    // POST: /api/courses/1/enrollments
    [HttpPost]
    public async Task<IActionResult> EnrollStudent(
        int courseId,
        EnrollStudentRequest request,
        CancellationToken ct)
    {

        // 1. Check course exists
        var course =
            await courseService.GetByIdAsync(
                courseId,
                ct);


        if (course is null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Course not found",
                Status = StatusCodes.Status404NotFound
            });
        }



        // 2. Check capacity
        if (course.EnrollmentCount >= course.MaxCapacity)
        {
            return Conflict(new ProblemDetails
            {
                Title = "Course is full",
                Detail = $"Course '{course.Name}' has reached its maximum capacity of {course.MaxCapacity}.",
                Status = StatusCodes.Status409Conflict
            });
        }



        // 3. Create enrollment
        var enrollment =
            await enrollmentService.CreateAsync(
                courseId,
                request,
                ct);



        return CreatedAtAction(
            nameof(GetEnrollment),
            new
            {
                courseId,
                id = enrollment.Id
            },
            enrollment);
    }

}