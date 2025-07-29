using SharedKernel;

namespace Domain.Silver.DimCategories;

public sealed class DimCategory : Entity
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; }
}
