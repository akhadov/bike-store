using Domain.Stores;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Stores;

internal sealed class StoreConfiguration : IEntityTypeConfiguration<Store>
{
    public void Configure(EntityTypeBuilder<Store> builder)
    {
        builder.ToTable("stores", Schemas.Bronze);

        builder.HasKey(s => s.StoreId);

        builder.Property(s => s.Phone).HasMaxLength(25).IsRequired();

        builder.HasIndex(s => s.Email).IsUnique();
    }
}
