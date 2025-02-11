using CWB.ProductionPlanWO.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Configurations
{
    public class ProcPlanPartPurChaseRelConfiguration : IEntityTypeConfiguration<ProcPlanPartPurChaseRel>
    {
        public void Configure(EntityTypeBuilder<ProcPlanPartPurChaseRel> builder)
        {
            builder
                .ToTable("ProcPlanPartPurChaseRel");
            builder
               .HasKey(c => c.Id);
            builder
                .Property(t => t.ProcPlanId)
                .HasColumnName("ProcPlanId")
                .IsRequired();
            builder
              .Property(t => t.PartPurchaseId)
              .HasColumnName("PartPurchaseId")
              .IsRequired();
            builder
               .Property(t => t.LeadTime)
               .HasColumnName("LeadTime")
               .IsRequired();
            builder
               .Property(t => t.Active)
               .HasColumnName("Active")
               .IsRequired();
        }
    }
}
