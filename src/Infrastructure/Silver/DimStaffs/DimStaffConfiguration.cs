using Domain.Silver.DimStaffs;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Silver.DimStaffs;

internal sealed class DimStaffConfiguration : IEntityTypeConfiguration<DimStaff>
{
    public void Configure(EntityTypeBuilder<DimStaff> builder)
    {
        builder.ToTable("dim_staffs", Schemas.Silver);

        builder.HasKey(ds => ds.StaffId);
    }
}
