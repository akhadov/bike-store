using SharedKernel;

namespace Domain.Orders;

public sealed class Order : Entity
{
    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public OrderStatus OrderStatus { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime? RequiredDate { get; set; }

    public DateTime? ShippedDate { get; set; }

    public int StoreId { get; set; }

    public int StaffId { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = [];
}
