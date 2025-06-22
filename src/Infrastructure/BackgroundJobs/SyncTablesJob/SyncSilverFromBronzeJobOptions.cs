using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.BackgroundJobs.SyncTablesJob;

public sealed class SyncSilverFromBronzeJobOptions
{
    public int IntervalInSeconds { get; init; }
}
