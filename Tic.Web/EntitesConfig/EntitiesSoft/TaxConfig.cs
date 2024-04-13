using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Tic.Shared.EntitiesSoft;

namespace Spi.Web.EntitesConfig.EntitiesSoft
{
    public class TaxConfig : IEntityTypeConfiguration<Tax>
    {
        public void Configure(EntityTypeBuilder<Tax> builder)
        {
            builder.HasKey(e => e.TaxId);
            builder.HasIndex("CorporateId", "TaxName").IsUnique();
            builder.HasIndex("CorporateId", "Rate").IsUnique();
        }
    }
}
