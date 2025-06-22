using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Abstractions.Messaging;
using Application.Brands.SyncBramds;
using Application.SyncBronzeFromCsv;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;

namespace Infrastructure.BackgroundJobs.SyncTablesJob;

[DisallowConcurrentExecution]
internal sealed class SyncSilverFromBronzeJob(
    ICommandHandler<SyncBrandsCommand> brandsHandler,
    ILogger<ProcessCsvSyncJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        CancellationToken cancellationToken = context.CancellationToken;

        logger.LogInformation("Starting SyncSilverFromBronzeJob");

        await brandsHandler.Handle(new SyncBrandsCommand(), cancellationToken);

        logger.LogInformation("Finished SyncSilverFromBronzeJob");
    }
}
