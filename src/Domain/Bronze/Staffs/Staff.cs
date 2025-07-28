using CsvHelper.Configuration.Attributes;
using SharedKernel;

namespace Domain.Bronze.Staffs;
public sealed class Staff : Entity
{
    [Name("staff_id")]
    public int StaffId { get; set; }

    [Name("first_name")]
    public string FirstName { get; set; }

    [Name("last_name")]
    public string LastName { get; set; }

    [Name("email")]
    public string Email { get; set; }

    [Name("phone")]
    public string? Phone { get; set; }

    [Name("active")]
    public bool Active { get; set; }

    [Name("store_id")]
    public int StoreId { get; set; }

    [Name("manager_id")]
    public int? ManagerId { get; set; }

    public Staff? Manager { get; set; }
    public ICollection<Staff> Subordinates { get; set; } = [];
}
