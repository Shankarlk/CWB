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
    public class TempWO_Wait_ListConfiguration : IEntityTypeConfiguration<TempWO_Wait_List>
    {
        public void Configure(EntityTypeBuilder<TempWO_Wait_List> builder)
        {
            builder
                .ToTable("TempWO_Wait_List");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.Wo_Id)
                .HasColumnName("Wo_Id");
            builder
                .Property(b => b.ActiveId)
                .HasColumnName("ActiveId");
            builder
                .Property(b => b.Mode)
                .HasColumnName("Mode");
            builder
                .Property(b => b.Allow_Routing_Chg)
                .HasColumnName("Allow_Routing_Chg");
            builder
                .Property(b => b.Total_TPT)
                .HasColumnName("Total_TPT");
            builder
                .Property(b => b.Rework_Wo)
                .HasColumnName("Rework_Wo");
            builder
                .Property(b => b.NC_Log_Ref)
                .HasColumnName("NC_Log_Ref");
            builder
                .Property(b => b.WO_Wait_Seq_No)
                .HasColumnName("WO_Wait_Seq_No");
            builder
                .Property(b => b.Plan_Start_Date)
                .HasColumnName("Plan_Start_Date");
            builder
                .Property(b => b.Plan_End_Date)
                .HasColumnName("Plan_End_Date");
            builder
                .Property(b => b.Plan_Simul_Qnty)
                .HasColumnName("Plan_Simul_Qnty");
            builder
                .Property(b => b.NoOfSimulation)
                .HasColumnName("NoOfSimulation");
            builder
                .Property(b => b.StopingId)
                .HasColumnName("StopingId");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("TempWO_Wait_List_TenantId");
        }
    }
}
