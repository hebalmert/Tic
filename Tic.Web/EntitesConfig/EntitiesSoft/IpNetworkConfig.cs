using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tic.Shared.EntitiesSoft;

namespace Tic.Web.EntitesConfig.EntitiesSoft
{
    public class IpNetworkConfig : IEntityTypeConfiguration<IpNetwork>
    {
        public void Configure(EntityTypeBuilder<IpNetwork> builder)
        {
            builder.HasKey(e => e.IpNetworkId);
            builder.HasIndex("Ip", "CorporateId").IsUnique();
        }
    }
}
