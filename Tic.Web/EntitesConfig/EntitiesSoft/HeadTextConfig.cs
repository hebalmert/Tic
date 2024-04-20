using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tic.Shared.EntitiesSoft;

namespace Tic.Web.EntitesConfig.EntitiesSoft
{
    public class HeadTextConfig : IEntityTypeConfiguration<HeadText>
    {
        public void Configure(EntityTypeBuilder<HeadText> builder)
        {
            builder.HasKey(e => e.HeadTextId);
            builder.HasIndex("CorporateId", "TextoEncabezado").IsUnique();
            builder.Property(e => e.TextoEncabezado).HasMaxLength(512);
        }
    }
}
