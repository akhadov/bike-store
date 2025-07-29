using Domain.Silver.DimStores;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Silver.DimStores;

internal sealed class DimStoreConfiguration : IEntityTypeConfiguration<DimStore>
{
    public void Configure(EntityTypeBuilder<DimStore> builder)
    {
        builder.ToTable("dim_stores", Schemas.Silver);

        builder.HasKey(ds => ds.StoreId);
    }
}
