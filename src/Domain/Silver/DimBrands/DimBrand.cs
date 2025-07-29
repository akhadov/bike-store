using SharedKernel;

namespace Domain.Silver.DimBrands;

public sealed class DimBrand : Entity
{
    public int BrandId { get; set; }

    public string BrandName { get; set; }
}
