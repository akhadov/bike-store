using SharedKernel;

namespace Domain.Stocks;

public sealed class Stok : Entity
{
    public int StoreId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }
}
