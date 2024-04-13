using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tic.Shared.EntitiesSoft;

namespace Tic.Web.EntitesConfig.EntitiesSoft
{
    public class ZoneConfig : IEntityTypeConfiguration<Zone>
    {
        public void Configure(EntityTypeBuilder<Zone> builder)
        {
            builder.HasKey(e => e.ZoneId);
            builder.HasIndex("StateId", "CityId", "ZoneName", "CorporateId").IsUnique();
            //Evitar el borrado en cascada
            builder.HasOne(e => e.State).WithMany(c => c.Zones).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.City).WithMany(c => c.Zones).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
