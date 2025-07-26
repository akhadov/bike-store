using CsvHelper.Configuration.Attributes;
using SharedKernel;

namespace Domain.Products;

public sealed class Product : Entity
{
    [Name("product_id")]
    public int ProductId { get; set; }

    [Name("product_name")]
    public string ProductName { get; set; }

    [Name("brand_id")]
    public int BrandId { get; set; }

    [Name("category_id")]
    public int CategoryId { get; set; }

    [Name("model_year")]
    public int ModelYear { get; set; }

    [Name("list_price")]
    public decimal ListPrice { get; set; }
}
