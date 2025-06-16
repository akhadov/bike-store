﻿using SharedKernel;

namespace Domain.Orders;

public sealed class OrderItem : Entity
{
    public int ItemId { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal ListPrice { get; set; }

    public decimal Discount { get; set; }
}
