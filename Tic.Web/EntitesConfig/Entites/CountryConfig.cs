using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tic.Shared.Entites;

namespace Tic.Web.EntitesConfig.Entites
{
    public class CountryConfig : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasKey(e => e.CountryId);
            builder.Property(e => e.Name).HasMaxLength(100);
        }
    }
}
