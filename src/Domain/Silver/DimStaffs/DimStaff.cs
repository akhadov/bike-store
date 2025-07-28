using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration.Attributes;
using Domain.Bronze.Staffs;
using SharedKernel;

namespace Domain.Silver.DimStaffs;

public sealed class DimStaff : Entity
{
    public int StaffId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string? Phone { get; set; }

    public bool Active { get; set; }

    public int StoreId { get; set; }

    public int? ManagerId { get; set; }

    public DimStaff? Manager { get; set; }
    public ICollection<DimStaff> Subordinates { get; set; } = [];
}
