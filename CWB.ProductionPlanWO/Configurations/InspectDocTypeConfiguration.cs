using CWB.CommonUtils.Common.Configurations;
using CWB.ProductionPlanWO.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Configurations
{
    public class InspectDocTypeConfiguration : IEntityTypeConfiguration<InspectDocType>
    {
        public void Configure(EntityTypeBuilder<InspectDocType> builder)
        {
            builder
            .ToTable("InspectDocType");
            builder
               .HasKey(c => c.Id);
            builder
                .Property(t => t.DocumentTypeId)
                .HasColumnName("DocumentTypeId")
                .IsRequired();
            builder
                .Property(t => t.Mandatory)
                .HasColumnName("Mandatory")
                .IsRequired();
            builder
                .Property(c => c.UpdatedBy)
                .HasColumnName("UpdatedBy")
                .IsRequired();
            builder
                .Property(c => c.UpdatedOn)
                .HasColumnName("UpdatedOn")
                .IsRequired();
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("InspectDocType_TenantId");
        }
    }
}
