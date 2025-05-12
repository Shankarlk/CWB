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
    public class Inw_Recpt_DetailsConfiguration : IEntityTypeConfiguration<Inw_Recpt_Details>
    {
        public void Configure(EntityTypeBuilder<Inw_Recpt_Details> builder)
        {
            builder
                .ToTable("Inw_Recpt_Details");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.Inw_Recpt_Header_Id)
                .HasColumnName("Inw_Recpt_Header_Id");
            builder
                .Property(b => b.Inw_Recpt_Part_No_Id)
                .HasColumnName("Inw_Recpt_Part_No_Id");
            builder
                .Property(b => b.Our_count)
                .HasColumnName("Our_count");
            builder
                .Property(b => b.Vendor_DC_count)
                .HasColumnName("Vendor_DC_count");
            builder
                .Property(b => b.Inward_Condition)
                .HasColumnName("Inward_Condition");
            builder
                .Property(b => b.Comment)
                .HasColumnName("Comment");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Inw_Recpt_Details_TenantId");
        }
    }
}
