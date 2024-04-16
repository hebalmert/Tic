using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tic.Shared.EntitiesSoft;

namespace Tic.Web.EntitesConfig.EntitiesSoft
{
    public class ServerCOnfig : IEntityTypeConfiguration<Server>
    {
        public void Configure(EntityTypeBuilder<Server> builder)
        {
            builder.HasKey(e => e.ServerId);
            builder.HasIndex("ServerName", "CorporateId").IsUnique();

            //Evitar el borrado en cascada
            builder.HasOne(e => e.IpNetwork).WithMany(c => c.Servers).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.Mark).WithMany(c => c.Servers).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.MarkModel).WithMany(c => c.Servers).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.Zone).WithMany(c => c.Servers).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
