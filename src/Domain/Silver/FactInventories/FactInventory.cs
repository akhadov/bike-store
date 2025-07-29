using SharedKernel;

namespace Domain.Silver.FactInventories;

public sealed class FactInventory : Entity
{
    public int StoreId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }
}
