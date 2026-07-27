using TmsApi;
public class EnrollmentWorker
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<EnrollmentWorker> _logger;

    public EnrollmentWorker(IServiceScopeFactory scopeFactory, ILogger<EnrollmentWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public void ProcessBatch()
    {
        using var scope = _scopeFactory.CreateScope();
        var enrollmentService = scope.ServiceProvider.GetRequiredService<IEnrollmentService>();
        var allEnrollments = enrollmentService.GetAllAsync().GetAwaiter().GetResult();

        _logger.LogInformation(
            "Processed enrollment batch with {EnrollmentCount} records",
            allEnrollments.Count);
    }
}
