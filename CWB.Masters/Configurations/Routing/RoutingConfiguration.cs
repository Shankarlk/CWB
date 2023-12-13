using CWB.CommonUtils.Common.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CWB.Masters.Configurations
{
    public class RoutingConfiguration
        : IEntityTypeConfiguration<Domain.Routing>
    {
        public void Configure(EntityTypeBuilder<Domain.Routing> builder)
        {
            builder
             .ToTable("Routing");
            builder
               .HasKey(m => m.Id);
            builder
                .Property(m => m.RoutingName)
                .HasColumnName("RoutingName")
                .HasMaxLength(300)
                .IsRequired();
            builder
                .Property(t => t.ManufacturedPartId)
                .HasColumnName("ManufacturedPartId")
                .IsRequired();
            builder
                .Property(m => m.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
        }
    }
}
