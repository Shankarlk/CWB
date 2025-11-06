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
    public class SetupVariationReasonConfiguration : IEntityTypeConfiguration<SetupVariationReason>
    {
        public void Configure(EntityTypeBuilder<SetupVariationReason> builder)
        {
            builder
                .ToTable("SetupVariationReason");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.SetupType)
                .HasColumnName("SetupType");
            builder
                .Property(b => b.Reason)
                .HasColumnName("Reason");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("SetupVariationReason_TenantId");
        }
    }
}
