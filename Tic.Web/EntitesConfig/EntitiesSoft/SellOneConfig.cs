using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tic.Shared.EntitiesSoft;

namespace Tic.Web.EntitesConfig.EntitiesSoft
{
    public class SellOneConfig : IEntityTypeConfiguration<SellOne>
    {
        public void Configure(EntityTypeBuilder<SellOne> builder)
        {
            builder.HasKey(e => e.SellOneId);
            builder.HasIndex("SellControl", "CorporateId").IsUnique();
            builder.HasIndex("SellOneId", "OrderTicketDetailId", "CorporateId").IsUnique();
            builder.Property(e => e.NamePlan).HasMaxLength(50);
            builder.Property(e => e.Date).HasColumnType("date");
            builder.Property(e => e.Rate).HasPrecision(15,2);
            builder.Property(e => e.SubTotal).HasPrecision(18, 2);
            builder.Property(e => e.Impuesto).HasPrecision(18, 2);
            builder.Property(e => e.Total).HasPrecision(18, 2);
            //Evitar el borrado en cascada
            builder.HasOne(e => e.PlanCategory).WithMany(c => c.SellOnes).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.Plan).WithMany(c => c.SellOnes).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.Server).WithMany(c => c.SellOnes).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.OrderTicketDetail).WithMany(c => c.SellOnes).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.Manager).WithMany(c => c.SellOnes).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
