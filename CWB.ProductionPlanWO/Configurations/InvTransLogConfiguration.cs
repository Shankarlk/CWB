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
    public class InvTransLogConfiguration : IEntityTypeConfiguration<Inv_Trans_Log>
    {
        public void Configure(EntityTypeBuilder<Inv_Trans_Log> builder)
        {
            builder
                .ToTable("Inv_Trans_Log");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.Dt_time)
                .HasColumnName("Dt_time");
            builder
                .Property(b => b.PersonId)
                .HasColumnName("PersonId");
            builder
                .Property(b => b.Input_Part_NoId)
                .HasColumnName("Input_Part_NoId");
            builder
                .Property(b => b.Input_Routing_Id)
                .HasColumnName("Input_Routing_Id");
            builder
                .Property(b => b.Input_Opr_No)
                .HasColumnName("Input_Opr_No");
            builder
                .Property(b => b.Output_Part_No)
                .HasColumnName("Output_Part_No");
            builder
                .Property(b => b.Output_Routing_Id)
                .HasColumnName("Output_Routing_Id");
            builder
                .Property(b => b.Output_Opr_No)
                .HasColumnName("Output_Opr_No");
            builder
                .Property(b => b.Wo_Id)
                .HasColumnName("Wo_Id");
            builder
                .Property(b => b.PO_No_Id)
                .HasColumnName("PO_No_Id");
            builder
                .Property(b => b.Transaction_Id)
                .HasColumnName("Transaction_Id");
            builder
                .Property(b => b.Qnty)
                .HasColumnName("Qnty");
            builder
                .Property(b => b.From_Location_Id)
                .HasColumnName("From_Location_Id");
            builder
                .Property(b => b.To_Location_Id)
                .HasColumnName("To_Location_Id");
            builder
                .Property(b => b.Part_Status)
                .HasColumnName("Part_Status");
            builder
                .Property(b => b.Ready_for_issue)
                .HasColumnName("Ready_for_issue");
            builder
                .Property(b => b.Movement_Started)
                .HasColumnName("Movement_Started");
            builder
                .Property(b => b.Movement_Compl)
                .HasColumnName("Movement_Compl");
            builder
                .Property(b => b.Qnty_Mismatch)
                .HasColumnName("Qnty_Mismatch");
            builder
                .Property(b => b.Qnty_mismatch_status)
                .HasColumnName("Qnty_mismatch_status");
            builder
                .Property(b => b.Qnty_Mismatch_Comment)
                .HasColumnName("Qnty_Mismatch_Comment");
            builder
                .Property(b => b.Qnty_Mismatch_Resolution)
                .HasColumnName("Qnty_Mismatch_Resolution");
            builder
                .Property(b => b.NC_Log_Id)
                .HasColumnName("NC_Log_Id");
            builder
                .Property(b => b.Our_DC_Ref)
                .HasColumnName("Our_DC_Ref");
            builder
                .Property(b => b.Our_RGP_Ref)
                .HasColumnName("Our_RGP_Ref");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder
                .Property(b => b.From_Loc_Flag)
                .HasColumnName("From_Loc_Flag");
            builder
               .Property(b => b.To_Loc_Flag)
               .HasColumnName("To_Loc_Flag");
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Inv_Trans_Log_TenantId");
        }
    }
}
