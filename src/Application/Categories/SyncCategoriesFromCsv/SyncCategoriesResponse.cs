namespace Application.Categories.SyncCategoriesFromCsv;
public sealed record SyncCategoriesResponse
{
    public int CategoryId { get; init; }
    public string CategoryName { get; init; }
}
