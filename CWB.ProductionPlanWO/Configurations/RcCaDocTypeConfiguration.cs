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
    public class RcCaDocTypeConfiguration : IEntityTypeConfiguration<RcCaDocType>
    {
        public void Configure(EntityTypeBuilder<RcCaDocType> builder)
        {
            builder
            .ToTable("RcCaDocType");
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
            builder.HasIndex(c => c.TenantId).HasDatabaseName("RcCaDocType_TenantId");
        }
    }
}
