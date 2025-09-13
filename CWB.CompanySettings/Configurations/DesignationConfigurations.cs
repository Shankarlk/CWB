using CWB.CommonUtils.Common.Configurations;
using CWB.CompanySettings.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CWB.CompanySettings.Configurations
{
    public class DesignationConfigurations : IEntityTypeConfiguration<Designation>
    {
        public void Configure(EntityTypeBuilder<Designation> builder)
        {

            builder
                  .ToTable("Designations");
            builder.ConfigureBase();
            builder
                .Property(w => w.Name)
                .HasColumnName("Name")
                .IsUnicode(true)
                .HasMaxLength(255)
                .IsRequired();
            builder
               .Property(m => m.TenantId)
               .HasColumnName("TenantId")
               .IsRequired();

            builder.HasData(
                new Designation { Id = 1, Name = "Management", TenantId = 1 },
                new Designation { Id = 2, Name = "Operator", TenantId = 1 }
            );
            builder.HasIndex(m => m.TenantId).HasDatabaseName("Designation_TenantId");
        }
    }
}
