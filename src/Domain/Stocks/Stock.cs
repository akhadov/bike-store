using CsvHelper.Configuration.Attributes;
using SharedKernel;

namespace Domain.Stocks;

public sealed class Stok : Entity
{
    [Name("store_id")]
    public int StoreId { get; set; }

    [Name("product_id")]
    public int ProductId { get; set; }

    [Name("quantity")]
    public int Quantity { get; set; }
}
