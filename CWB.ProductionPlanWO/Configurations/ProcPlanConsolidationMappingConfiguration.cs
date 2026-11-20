using CWB.CommonUtils.Common.Configurations;
using CWB.ProductionPlanWO.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CWB.ProductionPlanWO.Configurations
{
    public class ProcPlanConsolidationMappingConfiguration : IEntityTypeConfiguration<ProcPlanConsolidationMapping>
    {
        public void Configure(EntityTypeBuilder<ProcPlanConsolidationMapping> builder)
        {
            builder
                .ToTable("ProcPlanConsolidationMapping");
            builder
                .HasKey(p => p.Id);
            builder
                 .Property(p => p.ConsolidatedProcPlanId)
                 .HasColumnName("ConsolidatedProcPlanId")
                 .IsRequired();
            builder
                .Property(p => p.ProcPlanId)
                .HasColumnName("ProcPlanId")
                .IsRequired();
            builder
                .Property(p => p.WorkOrderId)
                .HasColumnName("WorkOrderId").
                 HasDefaultValue(0)
                .IsRequired();
            builder
                .Property(p => p.TenantId)
                .HasColumnName("TenantId")
                .HasDefaultValue(0);
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("ProcPlanConsolidationMapping_TenantId");

        }

    }
}
