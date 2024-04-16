using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tic.Shared.EntitiesSoft;

namespace Tic.Web.EntitesConfig.EntitiesSoft
{
    public class OrderTicketDetailConfig : IEntityTypeConfiguration<OrderTicketDetail>
    {
        public void Configure(EntityTypeBuilder<OrderTicketDetail> builder)
        {
            builder.HasKey(e => e.OrderTicketDetailId);
            builder.HasIndex("CorporateId", "Control").IsUnique();
            builder.Property(e => e.DateCreado).HasColumnType("date");
            builder.Property(e => e.Usuario).HasMaxLength(25);
            builder.Property(e => e.Clave).HasMaxLength(25);
            //Evitar el borrado en cascada
            builder.HasOne(e => e.OrderTickets).WithMany(c => c.OrderTicketDetails).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
