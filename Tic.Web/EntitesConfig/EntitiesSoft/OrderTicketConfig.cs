using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tic.Shared.EntitiesSoft;

namespace Tic.Web.EntitesConfig.EntitiesSoft
{
    public class OrderTicketConfig : IEntityTypeConfiguration<OrderTicket>
    {
        public void Configure(EntityTypeBuilder<OrderTicket> builder)
        {
            builder.HasKey(e => e.OrderTicketId);
            builder.HasIndex("CorporateId", "OrdenControl").IsUnique();
            builder.Property(e => e.NamePlan).HasMaxLength(50);
            builder.Property(e => e.Date).HasColumnType("date");
            builder.Property(e => e.Rate).HasPrecision(18, 2);
            builder.Property(e => e.Precio).HasPrecision(18, 2);
            builder.Property(e => e.Cantidad).HasPrecision(18, 2);
            builder.Property(e => e.SubTotal).HasPrecision(18, 2);
            builder.Property(e => e.Impuesto).HasPrecision(18, 2);
            builder.Property(e => e.Total).HasPrecision(18, 2);
            //Evitar el borrado en cascada
            builder.HasOne(e => e.PlanCategory).WithMany(c => c.OrderTickets).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.Plan).WithMany(c => c.OrderTickets).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.Server).WithMany(c => c.OrderTickets).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
