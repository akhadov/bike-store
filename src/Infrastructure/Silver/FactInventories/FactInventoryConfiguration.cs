using Domain.Silver.FactInventories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Silver.FactInventories;

internal sealed class FactInventoryConfiguration : IEntityTypeConfiguration<FactInventory>
{
    public void Configure(EntityTypeBuilder<FactInventory> builder)
    {
        builder.ToTable("fact_inventory", Schemas.Silver);

        builder.HasKey(fi =>  new {fi.StoreId, fi.ProductId});

    }
}
