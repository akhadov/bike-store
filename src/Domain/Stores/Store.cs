using SharedKernel;

namespace Domain.Stores;

public sealed class Store : Entity
{
    public int StoreId { get; set; }

    public string StoreName { get; set; }

    public string Phone { get; set; }

    public string Email { get; set; }

    public string Street { get; set; }

    public string City { get; set; }

    public string State { get; set; }

    public string ZipCode { get; set; }
}
