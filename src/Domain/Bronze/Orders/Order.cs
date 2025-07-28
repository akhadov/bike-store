using CsvHelper.Configuration.Attributes;
using SharedKernel;

namespace Domain.Bronze.Orders;

public sealed class Order : Entity
{
    [Name("order_id")]
    public int OrderId { get; set; }

    [Name("customer_id")]
    public int CustomerId { get; set; }

    [Name("order_status")]
    public OrderStatus OrderStatus { get; set; }

    [Name("order_date")]
    public DateTime OrderDate { get; set; }

    [Name("required_date")]
    public DateTime? RequiredDate { get; set; }

    [Name("shipped_date")]
    public DateTime? ShippedDate { get; set; }

    [Name("store_id")]
    public int StoreId { get; set; }

    [Name("staff_id")]
    public int StaffId { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = [];
}
