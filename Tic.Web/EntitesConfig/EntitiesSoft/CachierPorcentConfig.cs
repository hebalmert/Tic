using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tic.Shared.EntitiesSoft;

namespace Tic.Web.EntitesConfig.EntitiesSoft
{
    public class CachierPorcentConfig : IEntityTypeConfiguration<CachierPorcent>
    {
        public void Configure(EntityTypeBuilder<CachierPorcent> builder)
        {
            builder.HasKey(e => e.CachierPorcentId);
            builder.Property(e => e.Date).HasColumnType("date");
            builder.Property(e => e.NamePlan).HasMaxLength(100);
            builder.Property(e => e.Porcentaje).HasPrecision(18,2);
            builder.Property(e => e.Precio).HasPrecision(18, 2);
            builder.Property(e => e.Comision).HasPrecision(18, 2);
            builder.Property(e => e.DatePagado).HasColumnType("date");
            //Evitar el borrado en cascada
            builder.HasOne(e => e.Cachier).WithMany(c => c.CachierPorcents).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.SellOneCachier).WithMany(c => c.CachierPorcents).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.OrderTicketDetail).WithMany(c => c.CachierPorcents).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
