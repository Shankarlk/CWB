using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CWB.CommonUtils.Common.Configurations;
using CWB.ProductionPlanWO.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CWB.ProductionPlanWO.Configurations
{
    public class Rwk_ListConfiguration : IEntityTypeConfiguration<Rwk_List>
    {
        public void Configure(EntityTypeBuilder<Rwk_List> builder)
        {
            builder
                .ToTable("Rwk_List");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.NC_Log_Id)
                .HasColumnName("NC_Log_Id");
            builder
                .Property(b => b.Mc_Id)
                .HasColumnName("Mc_Id");
            builder
                .Property(b => b.Allocated)
                .HasColumnName("Allocated");
            builder
                .Property(b => b.Plan_Duration)
                .HasColumnName("Plan_Duration");
            builder
                .Property(b => b.RequiredStartTime)
                .HasColumnName("RequiredStartTime");
            builder
                .Property(b => b.Actual_Start_Time)
                .HasColumnName("Actual_Start_Time");
            builder
                .Property(b => b.Actual_end_Time)
                .HasColumnName("Actual_end_Time");
            builder
                .Property(b => b.Actual_Duration)
                .HasColumnName("Actual_Duration");
            builder
                .Property(b => b.Closure_Comment)
                .HasColumnName("Closure_Comment");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Rwk_List_TenantId");
        }
    }
}
