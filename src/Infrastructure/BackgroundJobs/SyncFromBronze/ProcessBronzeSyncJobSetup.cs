using Microsoft.Extensions.Options;
using Quartz;

namespace Infrastructure.BackgroundJobs.SyncFromBronze;
internal sealed class ProcessBronzeSyncJobSetup : IConfigureOptions<QuartzOptions>
{
    private readonly BronzeSyncJobOptions _bronzeSyncJobOptions;

    public ProcessBronzeSyncJobSetup(IOptions<BronzeSyncJobOptions> bronzeSyncJobOptions)
    {
        _bronzeSyncJobOptions = bronzeSyncJobOptions.Value;
    }

    public void Configure(QuartzOptions options)
    {
        const string jobName = nameof(ProcessBronzeSyncJob);

        options.AddJob<ProcessBronzeSyncJob>(configure => configure.WithIdentity(jobName))
            .AddTrigger(configure =>
                configure
                    .ForJob(jobName)
                    .WithSimpleSchedule(schedule =>
                        schedule.WithIntervalInSeconds(_bronzeSyncJobOptions.IntervalInSeconds).RepeatForever()));
    }
}
