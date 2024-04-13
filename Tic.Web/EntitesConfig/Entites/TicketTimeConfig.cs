using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tic.Shared.Entites;

namespace Tic.Web.EntitesConfig.Entites
{
    public class TicketTimeConfig : IEntityTypeConfiguration<TicketTime>
    {
        public void Configure(EntityTypeBuilder<TicketTime> builder)
        {
            builder.HasKey(e => e.TicketTimeId);
            builder.HasIndex("Tiempo").IsUnique();
            builder.Property(e => e.Tiempo).HasMaxLength(25);
        }
    }
}
