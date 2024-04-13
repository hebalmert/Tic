using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tic.Shared.EntitiesSoft;

namespace Tic.Web.EntitesConfig.EntitiesSoft
{
    public class ChainCodeConfig : IEntityTypeConfiguration<ChainCode>
    {
        public void Configure(EntityTypeBuilder<ChainCode> builder)
        {
            builder.HasKey(e => e.ChainCodeId);
            builder.Property(e => e.Cadena).HasMaxLength(36);
        }
    }
}
