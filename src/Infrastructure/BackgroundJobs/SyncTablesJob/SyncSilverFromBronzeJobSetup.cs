using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Quartz;

namespace Infrastructure.BackgroundJobs.SyncTablesJob;
internal sealed class SyncSilverFromBronzeJobSetup : IConfigureOptions<QuartzOptions>
{
    private readonly SyncSilverFromBronzeJobOptions _syncOptions;
    public SyncSilverFromBronzeJobSetup(IOptions<SyncSilverFromBronzeJobOptions> syncOptions)
    {
        _syncOptions = syncOptions.Value;
    }

    public void Configure(QuartzOptions options)
    {
        const string jobName = nameof(SyncSilverFromBronzeJob);

        options.AddJob<SyncSilverFromBronzeJob>(configure => configure.WithIdentity(jobName))
            .AddTrigger(configure =>
                configure
                    .ForJob(jobName)
                    .WithSimpleSchedule(schedule =>
                        schedule.WithIntervalInSeconds(_syncOptions.IntervalInSeconds).RepeatForever()));
    }
}
