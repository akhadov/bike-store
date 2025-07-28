using CsvHelper.Configuration.Attributes;
using SharedKernel;

namespace Domain.Bronze.Orders;

public sealed class OrderItem : Entity
{
    [Name("item_id")]
    public int ItemId { get; set; }

    [Name("order_id")]
    public int OrderId { get; set; }

    [Name("product_id")]
    public int ProductId { get; set; }

    [Name("quantity")]
    public int Quantity { get; set; }

    [Name("list_price")]
    public decimal ListPrice { get; set; }

    [Name("discount")]
    public decimal Discount { get; set; }
}
