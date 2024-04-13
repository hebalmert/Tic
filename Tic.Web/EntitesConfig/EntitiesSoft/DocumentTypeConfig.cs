using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tic.Shared.EntitiesSoft;

namespace Spi.Web.EntitiesConfig.EntitiesSoft
{
    public class DocumentTypeConfig : IEntityTypeConfiguration<DocumentType>
    {
        public void Configure(EntityTypeBuilder<DocumentType> builder)
        {
            builder.HasKey(e => e.DocumentTypeId);
            builder.HasIndex("DocumentName", "CorporateId").IsUnique();
        }
    }
}
