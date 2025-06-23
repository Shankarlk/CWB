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
    public class TempMc_Wait_ListConfiguration : IEntityTypeConfiguration<TempMc_Wait_List>
    {
        public void Configure(EntityTypeBuilder<TempMc_Wait_List> builder)
        {
            builder
                .ToTable("TempMc_Wait_List");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.Wo_Id)
                .HasColumnName("Wo_Id");
            builder
                .Property(b => b.ActiveId)
                .HasColumnName("ActiveId");
            builder
                .Property(b => b.Opr_No_Id)
                .HasColumnName("Opr_No_Id");
            builder
                .Property(b => b.Mc_Id)
                .HasColumnName("Mc_Id");
            builder
                .Property(b => b.Mode)
                .HasColumnName("Mode");
            builder
                .Property(b => b.Bal_Qnty)
                .HasColumnName("Bal_Qnty");
            builder
                .Property(b => b.Act_Qnty)
                .HasColumnName("Act_Qnty");
            builder
                .Property(b => b.Plan_Qnty)
                .HasColumnName("Plan_Qnty");
            builder
                .Property(b => b.Bal_Qnty)
                .HasColumnName("Bal_Qnty");
            builder
                .Property(b => b.Rework_Wo)
                .HasColumnName("Rework_Wo");
            builder
                .Property(b => b.Non_Plan_Wk)
                .HasColumnName("Non_Plan_Wk");
            builder
                .Property(b => b.Non_Plan_wk_Id)
                .HasColumnName("Non_Plan_wk_Id");
            builder
                .Property(b => b.Plan_start_time_Id)
                .HasColumnName("Plan_start_time_Id");
            builder
                .Property(b => b.Plan_end_time_Id)
                .HasColumnName("Plan_end_time_Id");
            builder
                .Property(b => b.Next_Opr_Start_time_Id)
                .HasColumnName("Next_Opr_Start_time_Id");
            builder
                .Property(b => b.Setup_Start_time)
                .HasColumnName("Setup_Start_time");
            builder
                .Property(b => b.Setup_Apprvl_time)
                .HasColumnName("Setup_Apprvl_time");
            builder
                .Property(b => b.Act_End_time)
                .HasColumnName("Act_End_time");
            builder
                .Property(b => b.Mc_TPT)
                .HasColumnName("Mc_TPT");
            builder
                .Property(b => b.StopingId)
                .HasColumnName("StopingId");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("TempMc_Wait_List_TenantId");
        }
    }
}
