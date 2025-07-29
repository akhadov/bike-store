namespace Infrastructure.BackgroundJobs.SyncFromCsv;
public sealed class CsvSyncJobOptions
{
    public int IntervalInSeconds { get; init; }
    public string CsvFolderPath { get; init; }
}
