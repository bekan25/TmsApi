using Microsoft.EntityFrameworkCore;
using TmsApi;
using TmsApi.Dtos;
using TmsApi.Entities;

namespace TmsApi.Services;

public class CourseService(TmsDbContext context) : ICourseService
{
    public async Task<CourseResponseDto?> GetByIdAsync(
        int id,
        CancellationToken ct)
    {
        return await context.Courses
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CourseResponseDto(
                c.Id,
                c.Code,
                c.Name,
                c.Description,
                c.CreditHours,
                c.MaxCapacity,
                c.Enrollments.Count))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<CourseResponseDto> CreateAsync(
        CreateCourseRequest request,
        CancellationToken ct)
    {
        var course = new Course
        {
            Code = request.Code,
            Name = request.Name,
            MaxCapacity = request.MaxCapacity
        };

        context.Courses.Add(course);

        await context.SaveChangesAsync(ct);

        return new CourseResponseDto(
            course.Id,
            course.Code,
            course.Name,
            course.Description,
            course.CreditHours,
            course.MaxCapacity,
            0);
    }

    public async Task<bool> CodeExistsAsync(
        string code,
        CancellationToken ct)
    {
        return await context.Courses
            .AnyAsync(c => c.Code == code, ct);
    }

    public async Task<PagedResponse<CourseResponseDto>> GetCoursesAsync(
        PagedRequest request,
        CancellationToken ct)
    {
        // 1. Start with a no-tracking query.
        IQueryable<Course> query = context.Courses
            .AsNoTracking();

        // 2. Apply search filter.
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();

            query = query.Where(c =>
                EF.Functions.ILike(c.Name, $"%{search}%")||
                EF.Functions.ILike(c.Code, $"%{search}%"));
        }

        // 3. Count BEFORE paging.
        var totalCount = await query.CountAsync(ct);

        // 4. Apply safe ordering.
        IQueryable<Course> sortedQuery = request.OrderBy.ToLowerInvariant() switch
{
    "code" => request.Descending
        ? query.OrderByDescending(c => c.Code)
        : query.OrderBy(c => c.Code),

    "maxcapacity" => request.Descending
        ? query.OrderByDescending(c => c.MaxCapacity)
        : query.OrderBy(c => c.MaxCapacity),

    "name" or "title" => request.Descending
        ? query.OrderByDescending(c => c.Name)
        : query.OrderBy(c => c.Name),

    _ => request.Descending
        ? query.OrderByDescending(c => c.Name)
        : query.OrderBy(c => c.Name)
};

        // 5. Apply paging and projection BEFORE materialising.
        var items = await sortedQuery
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(c => new CourseResponseDto(
                c.Id,
                c.Code,
                c.Name,
                c.Description,
                c.CreditHours,
                c.MaxCapacity,
                c.Enrollments.Count))
            .ToListAsync(ct);

        // 6. Return paginated response.
        return new PagedResponse<CourseResponseDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}