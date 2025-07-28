using CsvHelper.Configuration.Attributes;
using SharedKernel;

namespace Domain.Bronze.Brands;

public sealed class Brand : Entity
{
    [Name("brand_id")]
    public int BrandId { get; set; }

    [Name("brand_name")]
    public string BrandName { get; set; }
}
