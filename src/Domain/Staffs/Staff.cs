using SharedKernel;

namespace Domain.Staffs;
public sealed class Staff : Entity
{
    public int StaffId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string? Phone { get; set; }

    public bool Active { get; set; }

    public int StoreId { get; set; }

    public int? ManagerId { get; set; }

    public Staff? Manager { get; set; }
    public ICollection<Staff> Subordinates { get; set; } = [];
}
