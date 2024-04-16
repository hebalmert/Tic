using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tic.Shared.EntitiesSoft;

namespace Tic.Web.EntitesConfig.EntitiesSoft
{
    public class MarkModelConfig : IEntityTypeConfiguration<MarkModel>
    {
        public void Configure(EntityTypeBuilder<MarkModel> builder)
        {
            builder.HasKey(e => e.MarkModelId);
            builder.HasIndex("MarkModelName", "CorporateId", "MarkId").IsUnique();
            //Evitar el borrado en cascada
            builder.HasOne(e => e.Mark).WithMany(c => c.MarkModels).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
