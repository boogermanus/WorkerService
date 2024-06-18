
namespace WorkerService;

public class QueueHostedService : BackgroundService
{
    private IBackgroundTaskQueue _taskQueue;
    private ILogger _logger;
    public QueueHostedService(IBackgroundTaskQueue taskQueue, ILogger<QueueHostedService> logger)
    {
        _taskQueue = taskQueue;
        _logger = logger;
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("""
            {Name} is running.
            Tap W to add a work item the
            background queue
            """,nameof(QueueHostedService));

        return ProcessTaskQueueAsync(stoppingToken);
    }

    private async Task ProcessTaskQueueAsync(CancellationToken cancellationToken)
    {
        while(!cancellationToken.IsCancellationRequested)
        {
            try
            {
                Func<CancellationToken, ValueTask>? workItem = await _taskQueue.DequeueAsync(cancellationToken);

                await workItem(cancellationToken);
            }
            catch(OperationCanceledException) {}
            catch(Exception ex)
            {
                _logger.LogError(ex,"Error occurred executing task work item.");
            }
        }
    }
}