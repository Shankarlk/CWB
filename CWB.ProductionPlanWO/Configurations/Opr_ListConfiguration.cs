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
    public class Opr_ListConfiguration : IEntityTypeConfiguration<Opr_List>
    {
        public void Configure(EntityTypeBuilder<Opr_List> builder)
        {
            builder
                .ToTable("Opr_List");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.Wo_Id)
                .HasColumnName("Wo_Id");
            builder
                .Property(b => b.Opr_No)
                .HasColumnName("Opr_No");
            builder
               .Property(b => b.RoutingId)
               .HasColumnName("RoutingId");
            builder
               .Property(b => b.RoutingStepSequence)
               .HasColumnName("RoutingStepSequence");
            builder
               .Property(b => b.RoutingStepLocation)
               .HasColumnName("RoutingStepLocation");
            builder
                .Property(b => b.Mode)
                .HasColumnName("Mode");
            builder
                .Property(b => b.Initial_Opr_TPT)
                .HasColumnName("Initial_Opr_TPT");
            builder
                .Property(b => b.Rolledup_Opr_TPT)
                .HasColumnName("Rolledup_Opr_TPT");
            builder
                .Property(b => b.Rework_Wo)
                .HasColumnName("Rework_Wo");
            builder
                .Property(b => b.NC_Log_Ref)
                .HasColumnName("NC_Log_Ref");
            builder
                .Property(b => b.Act_Qnty)
                .HasColumnName("Act_Qnty");
            builder
                .Property(b => b.Plan_Qnty)
                .HasColumnName("Plan_Qnty");
            builder
                .Property(b => b.No_of_Simult_Mcs)
                .HasColumnName("No_of_Simult_Mcs");
            builder
                .Property(b => b.Shop_Plan_start_time)
                .HasColumnName("Shop_Plan_start_time");
            builder
                .Property(b => b.Shop_Plan_end_time)
                .HasColumnName("Shop_Plan_end_time");
            builder
                .Property(b => b.Subcon_plan_start_time)
                .HasColumnName("Subcon_plan_start_time");
            builder
                .Property(b => b.Subcon_plan_end_time)
                .HasColumnName("Subcon_plan_end_time");
            builder
                .Property(b => b.Setup_Start_time)
                .HasColumnName("Setup_Start_time");
            builder
                .Property(b => b.Act_End_time)
                .HasColumnName("Act_End_time");
            builder
                .Property(b => b.StopingId)
                .HasColumnName("StopingId");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Opr_List_TenantId");
        }
    }
}
