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
    public class Non_Plan_Wk_ListConfiguration : IEntityTypeConfiguration<Non_Plan_Wk_List>
    {
        public void Configure(EntityTypeBuilder<Non_Plan_Wk_List> builder)
        {
            builder
                .ToTable("Non_Plan_Wk_List");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.Mc_Id)
                .HasColumnName("Mc_Id");
            builder
                .Property(b => b.NP_Work_Type)
                .HasColumnName("NP_Work_Type");
            builder
                .Property(b => b.Allocated)
                .HasColumnName("Allocated");
            builder
                .Property(b => b.Plan_start_time)
                .HasColumnName("Plan_start_time");
            builder
                .Property(b => b.Plan_Duration)
                .HasColumnName("Plan_Duration");
            builder
                .Property(b => b.Non_Plan_Wk_Type)
                .HasColumnName("Non_Plan_Wk_Type");
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
                .Property(b => b.Work_Description)
                .HasColumnName("Work_Description");
            builder
                .Property(b => b.Closure_Comment)
                .HasColumnName("Closure_Comment");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Non_Plan_Wk_List_TenantId");
        }
    }
}
