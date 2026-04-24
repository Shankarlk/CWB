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
    public class ConsolidatedWoMappingConfiguration : IEntityTypeConfiguration<ConsolidatedWoMapping>
    {
        public void Configure(EntityTypeBuilder<ConsolidatedWoMapping> builder)
        {
            builder
                .ToTable("ConsolidatedWoMapping");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.CombinedWoId)
                .HasColumnName("CombinedWoId");
            builder
               .Property(b => b.WoId)
               .HasColumnName("WoId");
            builder
                 .Property(b => b.ParentWoId)
                 .HasColumnName("ParentWoId");
            builder
               .Property(c => c.TenantId)
               .HasColumnName("TenantId")
               .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("ConsolidatedWoMapping_TenantId");

        }
    }
}
