using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;
using SharedKernel;

namespace Domain.Silver.FactInventories;

public sealed class FactInventory : Entity
{
    public int StoreId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }
}
