using Microsoft.AspNetCore.Mvc;
using TmsApi;
using TmsApi.Entities;

namespace TmsApi.Controllers;

[ApiController]
[Route("api/enrollments")]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService enrollmentService;

    public EnrollmentsController(IEnrollmentService enrollmentService)
    {
        this.enrollmentService = enrollmentService;
    }


    // GET: api/enrollments
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var enrollments = await enrollmentService.GetAllAsync();

        return Ok(enrollments);
    }


    // GET: api/enrollments/1
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var enrollment = await enrollmentService.GetByIdAsync(id);

        if (enrollment == null)
            return NotFound();

        return Ok(enrollment);
    }


    // POST: api/enrollments
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateEnrollmentRequest request)
    {
        var enrollment = await enrollmentService.EnrollAsync(
            request.StudentId,
            request.CourseId
        );


        return CreatedAtAction(
            nameof(GetById),
            new { id = enrollment.Id },
            enrollment
        );
    }


    // DELETE: api/enrollments/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await enrollmentService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}


// Request DTO
public record CreateEnrollmentRequest(
    int StudentId,
    int CourseId
);