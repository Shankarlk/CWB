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
    public class Inw_Recpt_HeaderConfiguration : IEntityTypeConfiguration<Inw_Recpt_Header>
    {
        public void Configure(EntityTypeBuilder<Inw_Recpt_Header> builder)
        {
            builder
                .ToTable("Inw_Recpt_Header");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.PoHeaderId)
                .HasColumnName("PoHeaderId");
            builder
                .Property(b => b.Supplier_Dc_Ref)
                .HasColumnName("Supplier_Dc_Ref");
            builder
                .Property(b => b.Supplier_Dc_Date)
                .HasColumnName("Supplier_Dc_Date");
            builder
                .Property(b => b.Supplier_Inv_ref)
                .HasColumnName("Supplier_Inv_ref");
            builder
                .Property(b => b.Supplier_Inv_date)
                .HasColumnName("Supplier_Inv_date");
            builder
                .Property(b => b.Inw_Date_time)
                .HasColumnName("Inw_Date_time");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Inw_Recpt_Header_TenantId");
        }
    }
}
