using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tic.Shared.Entites;

namespace Tic.Web.EntitesConfig.Entites
{
    public class CityConfig : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.HasKey(e => e.CityId);
            builder.Property(e => e.Name).HasMaxLength(100);
            //Evitar el borrado en cascada
            builder.HasOne(e => e.State).WithMany(c => c.Cities).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
