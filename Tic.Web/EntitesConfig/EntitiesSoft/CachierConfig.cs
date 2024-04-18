using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tic.Shared.EntitiesSoft;

namespace Tic.Web.EntitesConfig.EntitiesSoft
{
    public class CachierConfig : IEntityTypeConfiguration<Cachier>
    {
        public void Configure(EntityTypeBuilder<Cachier> builder)
        {
            builder.HasKey(e => e.CachierId);
            builder.HasIndex("CorporateId", "FullName").IsUnique();
            builder.HasIndex("CorporateId", "Document").IsUnique();
            builder.HasIndex("UserName").IsUnique();
            builder.Property(e => e.FirstName).HasMaxLength(50);
            builder.Property(e => e.LastName).HasMaxLength(50);
            builder.Property(e => e.FullName).HasMaxLength(100);
            builder.Property(e => e.Document).HasMaxLength(25);
            builder.Property(e => e.UserName).HasMaxLength(256);
            builder.Property(e => e.PhoneNumber).HasMaxLength(25);
            builder.Property(e => e.Address).HasMaxLength(256);
            builder.Property(e => e.RateCachier).HasPrecision(15, 2);
            //Evitar el borrado en cascada
            builder.HasOne(e => e.DocumentType).WithMany(c => c.Cachiers).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.Server).WithMany(c => c.Cachiers).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
