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
    public class Insp_Outcome_DetailsCOnfiguration : IEntityTypeConfiguration<Insp_Outcome_Details>
    {
        public void Configure(EntityTypeBuilder<Insp_Outcome_Details> builder)
        {
            builder
                .ToTable("Nc_Log");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.Inw_Recpt_Header_Id)
                .HasColumnName("Inw_Recpt_Header_Id");
            builder
                .Property(b => b.Inw_Recpt_Part_No_Id)
                .HasColumnName("Inw_Recpt_Part_No_Id");
            builder
                .Property(b => b.McWaitId)
                .HasColumnName("McWaitId");
            builder
                .Property(b => b.Inw_Insp_Log_Id)
                .HasColumnName("Inw_Insp_Log_Id");
            builder
                .Property(b => b.Shop_Insp_log_Id)
                .HasColumnName("Shop_Insp_log_Id");
            builder
                .Property(b => b.Final_Insp_log_Id)
                .HasColumnName("Final_Insp_log_Id");
            builder
                .Property(b => b.NC_Tracking_No)
                .HasColumnName("NC_Tracking_No");
            builder
                .Property(b => b.Balloon_No_Dir)
                .HasColumnName("Balloon_No_Dir");
            builder
                .Property(b => b.Balloon_No)
                .HasColumnName("Balloon_No");
            builder
                .Property(b => b.Feature_Descrip)
                .HasColumnName("Feature_Descrip");
            builder
                .Property(b => b.NC_Descrip)
                .HasColumnName("NC_Descrip");
            builder
                .Property(b => b.NC_Qnty)
                .HasColumnName("NC_Qnty");
            builder
                .Property(b => b.Decl_by_Supplier)
                .HasColumnName("Decl_by_Supplier");
            builder
                .Property(b => b.Storage_Location)
                .HasColumnName("Storage_Location");
            builder
                .Property(b => b.Label_Type)
                .HasColumnName("Label_Type");
            builder
                .Property(b => b.NC_Log_status_Id)
                .HasColumnName("NC_Log_status_Id");
            builder
                .Property(b => b.Routing_Id)
                .HasColumnName("Routing_Id");
            builder
                .Property(b => b.Opr_No_Id)
                .HasColumnName("Opr_No_Id");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Nc_Log_TenantId");
        }
    }
}
