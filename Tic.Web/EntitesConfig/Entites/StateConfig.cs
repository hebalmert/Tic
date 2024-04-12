using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tic.Shared.Entites;

namespace Tic.Web.EntitesConfig.Entites
{
    public class StateConfig : IEntityTypeConfiguration<State>
    {
        public void Configure(EntityTypeBuilder<State> builder)
        {
            builder.HasKey(e => e.StateId);
            builder.Property(e => e.Name).HasMaxLength(100);
            //Evitar el borrado en cascada
            builder.HasOne(e => e.Country).WithMany(c => c.States).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
