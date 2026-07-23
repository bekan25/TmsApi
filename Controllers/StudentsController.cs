using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TmsApi.Entities;

namespace TmsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly TmsDbContext _context;

    public StudentsController(TmsDbContext context)
    {
        _context = context;
    }

    // GET: api/students
    [HttpGet]
    public async Task<IActionResult> GetStudents()
    {
        var students = await _context.Students.ToListAsync();

        return Ok(students);
    }

    // GET: api/students/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStudent(int id)
    {
        var student = await _context.Students.FindAsync(id);

        if (student == null)
        {
            return NotFound(new
            {
                message = $"Student with ID {id} was not found."
            });
        }

        return Ok(student);
    }
}