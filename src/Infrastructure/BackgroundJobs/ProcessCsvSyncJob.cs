using Application.Abstractions.Messaging;
using Application.SyncBronzeFromCsv;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;

namespace Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
internal sealed class ProcessCsvSyncJob(
    ICommandHandler<SyncBronzeFromCsvCommand> handler,
    IOptions<CsvSyncJobOptions> csvSyncJobOptions,
    ILogger<ProcessCsvSyncJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        CancellationToken cancellationToken = context.CancellationToken;

        logger.LogInformation("Beginning to process CSV sync job");

        foreach (string file in Directory.GetFiles(csvSyncJobOptions.Value.CsvFolderPath, "*.csv"))
        {
            var command = new SyncBronzeFromCsvCommand(file);

            try
            {
                await handler.Handle(command, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing file: {FileName}", file);
            }
        }

        logger.LogInformation("Completed processing CSV sync job");
    }
}
