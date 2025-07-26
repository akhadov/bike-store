using Domain.Staffs;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Staffs;

internal sealed class StaffConfiguration : IEntityTypeConfiguration<Staff>
{
    public void Configure(EntityTypeBuilder<Staff> builder)
    {
        builder.ToTable("staffs", Schemas.Bronze);

        builder.HasKey(s => s.StaffId);

        builder.Property(s => s.FirstName).HasMaxLength(250);

        builder.Property(s => s.LastName).HasMaxLength(250);

        builder.HasIndex(s => s.Email).IsUnique();

        builder.Property(s => s.Phone).HasMaxLength(25);

        builder.HasOne(s => s.Manager)
            .WithMany(s => s.Subordinates)
            .HasForeignKey(s => s.ManagerId);
    }
}
