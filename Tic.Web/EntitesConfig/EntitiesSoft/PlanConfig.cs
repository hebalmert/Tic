using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tic.Shared.EntitiesSoft;

namespace Tic.Web.EntitesConfig.EntitiesSoft
{
    public class PlanConfig : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.HasKey(e => e.PlanId);
            builder.HasIndex("CorporateId", "PlanName", "ServerId").IsUnique();
            builder.Property(e => e.DateCreated).HasColumnType("date");
            builder.Property(e => e.DateEdit).HasColumnType("date");
            //Evitar el borrado en cascada
            builder.HasOne(e => e.PlanCategory).WithMany(c => c.Plans).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.Tax).WithMany(c => c.Plans).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.Server).WithMany(c => c.Plans).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.TicketInactive).WithMany(c => c.Plans).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.TicketRefresh).WithMany(c => c.Plans).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.TicketTime).WithMany(c => c.Plans).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
