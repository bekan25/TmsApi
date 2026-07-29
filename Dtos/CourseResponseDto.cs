namespace TmsApi.Dtos;

public record CourseResponseDto(
    int Id,
    string Code,
    string Name,
    string? Description,
    int CreditHours,
    int MaxCapacity,
    int EnrollmentCount);