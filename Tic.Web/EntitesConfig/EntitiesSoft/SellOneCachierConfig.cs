using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tic.Shared.EntitiesSoft;

namespace Tic.Web.EntitesConfig.EntitiesSoft
{
    public class SellOneCachierConfig : IEntityTypeConfiguration<SellOneCachier>
    {
        public void Configure(EntityTypeBuilder<SellOneCachier> builder)
        {
            builder.HasKey(e => e.SellOneCachierId);
            builder.HasIndex("CorporateId", "SellControl").IsUnique();
            builder.HasIndex("SellOneCachierId", "OrderTicketDetailId", "CorporateId").IsUnique();
            builder.Property(e => e.Date).HasColumnType("date");
            builder.Property(e => e.Rate).HasPrecision(15, 2);
            builder.Property(e => e.SubTotal).HasPrecision(18, 2);
            builder.Property(e => e.Impuesto).HasPrecision(18, 2);
            builder.Property(e => e.Total).HasPrecision(18, 2);
            //Evitar el borrado en cascada
            builder.HasOne(e => e.Cachier).WithMany(c => c.SellOneCachiers).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.PlanCategory).WithMany(c => c.SellOneCachiers).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.Plan).WithMany(c => c.SellOneCachiers).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.Server).WithMany(c => c.SellOneCachiers).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.OrderTicketDetail).WithMany(c => c.SellOneCachiers).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
