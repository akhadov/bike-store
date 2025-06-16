using SharedKernel;

namespace Domain.Brands;

public sealed class Brand : Entity
{
    public int BrandId { get; set; }

    public string BrandName { get; set; }
}
