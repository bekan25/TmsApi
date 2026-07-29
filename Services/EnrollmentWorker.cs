using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TmsApi.Services;

public class EnrollmentWorker(
    IEnrollmentService enrollmentService,
    ILogger<EnrollmentWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var enrollments = await enrollmentService.GetAllAsync(
                    stoppingToken);

                logger.LogInformation(
                    "Enrollment worker checked {Count} enrollments.",
                    enrollments.Count());
            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "An error occurred while processing enrollments.");
            }

            await Task.Delay(
                TimeSpan.FromMinutes(5),
                stoppingToken);
        }
    }
}