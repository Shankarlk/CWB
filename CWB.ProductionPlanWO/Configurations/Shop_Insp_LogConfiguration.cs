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
    public class Shop_Insp_LogConfiguration : IEntityTypeConfiguration<Shop_Insp_Log>
    {
        public void Configure(EntityTypeBuilder<Shop_Insp_Log> builder)
        {
            builder
                .ToTable("Shop_Insp_Log");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.Operator_Id)
                .HasColumnName("Operator_Id");
            builder
                .Property(b => b.Mc_Reference)
                .HasColumnName("Mc_Reference");
            builder
                .Property(b => b.Inspected_by)
                .HasColumnName("Inspected_by");
            builder
                .Property(b => b.Inspected_on)
                .HasColumnName("Inspected_on");
            builder
                .Property(b => b.Qnty_OK_finished)
                .HasColumnName("Qnty_OK_finished");
            builder
                .Property(b => b.Qnty_ok_input)
                .HasColumnName("Qnty_ok_input");
            builder
                .Property(b => b.Input_Part_No)
                .HasColumnName("Input_Part_No");
            builder
                .Property(b => b.Input_Routing_Id)
                .HasColumnName("Input_Routing_Id");
            builder
                .Property(b => b.Input_Opr_NoId)
                .HasColumnName("Input_Opr_NoId");
            builder
                .Property(b => b.Label_Type)
                .HasColumnName("Label_Type");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Shop_Insp_Log_TenantId");
        }
    }
}
