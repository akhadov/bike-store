using Application.Abstractions.Messaging;
using Application.Brands.SyncBramds;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;

namespace Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
internal sealed class ProcessCsvSyncJob(
    ICommandHandler<SyncBrandsCommand> brandsHandler,
    IOptions<CsvSyncJobOptions> csvSyncJobOptions,
    ILogger<ProcessCsvSyncJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        CancellationToken cancellationToken = context.CancellationToken;

        logger.LogInformation("Beginning to process CSV sync job");

        await brandsHandler.Handle(new SyncBrandsCommand(Path.Combine(csvSyncJobOptions.Value.CsvFolderPath, "brands.csv")), cancellationToken);

        logger.LogInformation("Completed processing CSV sync job");
    }
}
