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
    public class SubCon_ListConfiguration : IEntityTypeConfiguration<SubCon_List>
    {
        public void Configure(EntityTypeBuilder<SubCon_List> builder)
        {
            builder
                .ToTable("SubCon_List");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.Wo_Id)
                .HasColumnName("Wo_Id");
            builder
                .Property(b => b.Opr_No)
                .HasColumnName("Opr_No");
            builder
                .Property(b => b.Supplier_Id)
                .HasColumnName("Supplier_Id");
            builder
                .Property(b => b.Mode)
                .HasColumnName("Mode");
            builder
                .Property(b => b.Act_Qnty)
                .HasColumnName("Act_Qnty");
            builder
                .Property(b => b.Plan_Qnty)
                .HasColumnName("Plan_Qnty");
            builder
                .Property(b => b.Loaded)
                .HasColumnName("Loaded");
            builder
                .Property(b => b.Rework_Wo)
                .HasColumnName("Rework_Wo");
            builder
                .Property(b => b.Plan_Disp_date)
                .HasColumnName("Plan_Disp_date");
            builder
                .Property(b => b.Plan_Recpt_date)
                .HasColumnName("Plan_Recpt_date");
            builder
                .Property(b => b.Act_Disp_date)
                .HasColumnName("Act_Disp_date");
            builder
                .Property(b => b.Act_Recpt_date)
                .HasColumnName("Act_Recpt_date");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("SubCon_List_TenantId");
        }
    }
}
