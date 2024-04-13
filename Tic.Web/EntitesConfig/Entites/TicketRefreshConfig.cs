using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tic.Shared.Entites;

namespace Tic.Web.EntitesConfig.Entites
{
    public class TicketRefreshConfig : IEntityTypeConfiguration<TicketRefresh>
    {
        public void Configure(EntityTypeBuilder<TicketRefresh> builder)
        {
            builder.HasKey(e => e.TicketRefreshId);
            builder.HasIndex("Tiempo").IsUnique();
            builder.Property(e => e.Tiempo).HasMaxLength(25);
        }
    }
}
