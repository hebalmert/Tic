using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tic.Shared.EntitiesSoft;

namespace Tic.Web.EntitesConfig.EntitiesSoft
{ 
    public class SellPackDetailConfig : IEntityTypeConfiguration<SellPackDetail>
    {
        public void Configure(EntityTypeBuilder<SellPackDetail> builder)
        {
            builder.HasKey(e => e.SellPackDetailId);
            builder.HasIndex("CorporateId", "SellPackDetailId", "OrderTicketDetailId").IsUnique();

            //Evitar el borrado en cascada
            builder.HasOne(e => e.OrderTicketDetail).WithMany(c => c.SellPackDetails).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.SellPack).WithMany(c => c.SellPackDetails).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
