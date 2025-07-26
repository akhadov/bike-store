using CsvHelper.Configuration.Attributes;
using SharedKernel;

namespace Domain.Stores;

public sealed class Store : Entity
{
    [Name("store_id")]
    public int StoreId { get; set; }

    [Name("store_name")]
    public string StoreName { get; set; }

    [Name("phone")]
    public string Phone { get; set; }

    [Name("email")]
    public string Email { get; set; }

    [Name("street")]
    public string Street { get; set; }

    [Name("city")]
    public string City { get; set; }

    [Name("state")]
    public string State { get; set; }

    [Name("zip_code")]
    public string ZipCode { get; set; }
}
