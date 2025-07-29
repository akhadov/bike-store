using Microsoft.Extensions.Options;
using Quartz;

namespace Infrastructure.BackgroundJobs.SyncFromCsv;
internal sealed class ProcessCsvSyncJobSetup : IConfigureOptions<QuartzOptions>
{
    private readonly CsvSyncJobOptions _csvSyncJobOptions;

    public ProcessCsvSyncJobSetup(IOptions<CsvSyncJobOptions> csvSyncJobOptions)
    {
        _csvSyncJobOptions = csvSyncJobOptions.Value;
    }

    public void Configure(QuartzOptions options)
    {
        const string jobName = nameof(ProcessCsvSyncJob);

        options.AddJob<ProcessCsvSyncJob>(configure => configure.WithIdentity(jobName))
            .AddTrigger(configure =>
                configure
                    .ForJob(jobName)
                    .WithSimpleSchedule(schedule =>
                        schedule.WithIntervalInSeconds(_csvSyncJobOptions.IntervalInSeconds).RepeatForever()));
    }
}
