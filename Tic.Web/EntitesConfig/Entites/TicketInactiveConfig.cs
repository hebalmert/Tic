using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tic.Shared.Entites;

namespace Tic.Web.EntitesConfig.Entites
{
    public class TicketInactiveConfig : IEntityTypeConfiguration<TicketInactive>
    {
        public void Configure(EntityTypeBuilder<TicketInactive> builder)
        {
            builder.HasKey(e => e.TicketInactiveId);
            builder.HasIndex("Tiempo").IsUnique();
            builder.Property(e => e.Tiempo).HasMaxLength(25);
        }
    }
}
