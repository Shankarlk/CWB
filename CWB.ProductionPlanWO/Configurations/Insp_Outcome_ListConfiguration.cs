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
    public class Insp_Outcome_ListConfiguration : IEntityTypeConfiguration<Insp_Outcome_List>
    {
        public void Configure(EntityTypeBuilder<Insp_Outcome_List> builder)
        {
            builder
                .ToTable("Insp_Outcome_List");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.Insp_Outcome_desc)
                .HasColumnName("Insp_Outcome_desc");
            builder
                .Property(b => b.Applicability)
                .HasColumnName("Applicability");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Insp_Outcome_List_TenantId");
        }
    }
}
