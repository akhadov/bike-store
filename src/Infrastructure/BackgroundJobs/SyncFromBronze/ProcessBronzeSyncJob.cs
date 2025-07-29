using Application.Abstractions.Messaging;
using Application.Silver.FactSales.SyncSales;
using Application.Silver.FactInventories.SyncInventories;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Infrastructure.BackgroundJobs.SyncFromBronze;

[DisallowConcurrentExecution]
internal sealed class ProcessBronzeSyncJob(
    ICommandHandler<SyncSalesCommand> salesHandler,
    ICommandHandler<SyncInventoriesCommand> inventoriesHandler,
    ILogger<ProcessBronzeSyncJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        CancellationToken cancellationToken = context.CancellationToken;

        logger.LogInformation("Beginning to process Bronze sync job");

        await salesHandler.Handle(new SyncSalesCommand(), cancellationToken);

        await inventoriesHandler.Handle(new SyncInventoriesCommand(), cancellationToken);

        logger.LogInformation("ProcessBronzeSyncJob completed successfully");
    }
}
