using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedKernel;

namespace Domain.Silver.FactSales;

public sealed class FactSale : Entity
{
    public int OrderId { get; set; }

    public int ItemId { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime ShippedDate { get; set; }

    public int StoreId { get; set; }

    public int StaffId { get; set; }

    public int CustomerId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal ListPrice { get; set; }

    public decimal Discount { get; set; }

    public decimal TotalPrice { get; set; }
}
