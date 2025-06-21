namespace Application.Brands.SyncBramdsFromCsv;
public sealed record BrandCsvModel
{
    public int BrandId { get; init; }

    public string BrandName { get; init; }
}
