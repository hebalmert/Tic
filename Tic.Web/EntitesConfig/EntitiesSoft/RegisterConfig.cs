using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Tic.Shared.EntitiesSoft;

namespace Spi.Web.EntitesConfig.EntitiesSoft
{
    public class RegisterConfig : IEntityTypeConfiguration<Register>
    {
        public void Configure(EntityTypeBuilder<Register> builder)
        {
            builder.HasKey(e => e.RegisterId);
        }
    }
}
