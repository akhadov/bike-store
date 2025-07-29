using Application.Abstractions.Messaging;
using Application.Silver.FactSales.SyncSales;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Infrastructure.BackgroundJobs.SyncFromBronze;

[DisallowConcurrentExecution]
internal sealed class ProcessBronzeSyncJob(
    ICommandHandler<SyncSalesCommand> salesHandler,
    ILogger<ProcessBronzeSyncJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        CancellationToken cancellationToken = context.CancellationToken;

        logger.LogInformation("Beginning to process Bronze sync job");

        logger.LogInformation("Processing Silver FactSales Sync");

        await salesHandler.Handle(new SyncSalesCommand(), cancellationToken);

        logger.LogInformation("Completed processing Silver FactSales Sync");
    }
}
