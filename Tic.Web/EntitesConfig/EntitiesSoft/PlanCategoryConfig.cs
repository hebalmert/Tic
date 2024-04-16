using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tic.Shared.EntitiesSoft;

namespace Tic.Web.EntitesConfig.EntitiesSoft
{
    public class PlanCategoryConfig : IEntityTypeConfiguration<PlanCategory>
    {
        public void Configure(EntityTypeBuilder<PlanCategory> builder)
        {
            builder.HasKey(e => e.PlanCategoryId);
            builder.HasIndex("PlanCategoryName", "CorporateId").IsUnique();
        }
    }
}
