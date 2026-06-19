using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CWB.CommonUtils.Common.Configurations;
using CWB.Gro.Domain;

namespace CWB.Gro.Configurations
{
    public class Printout_formatConfiguration : IEntityTypeConfiguration<Printout_format>
    {
        public void Configure(EntityTypeBuilder<Printout_format> builder)
        {
            builder
                .ToTable("Printout_format");
            builder
                .HasKey(w => w.Id);
            builder
              .Property(t => t.CWB_Customer)
              .HasColumnName("CWB_Customer");
            builder
                .Property(t => t.Template_Location)
                .HasColumnName("Template_Location");
            builder
                .Property(t => t.Purpose)
                .HasColumnName("Purpose");

            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Printout_format_TenantId");
        }
    }
}
