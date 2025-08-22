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
    public class DispatchDetailsConfiguration : IEntityTypeConfiguration<DispatchDetails>
    {
        public void Configure(EntityTypeBuilder<DispatchDetails> builder)
        {
            builder
                .ToTable("DispatchDetails");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.SaleOrderId)
                .HasColumnName("SaleOrderId");
            builder
                .Property(b => b.CustomerId)
                .HasColumnName("CustomerId");
            builder
                .Property(b => b.PoNoId)
                .HasColumnName("PoNoId");
            builder
                .Property(b => b.PartNoId)
                .HasColumnName("PartNoId");
            builder
                .Property(b => b.InvoiceNo)
                .HasColumnName("InvoiceNo");
            builder
                .Property(b => b.NoOfParts)
                .HasColumnName("NoOfParts");
            builder
                .Property(b => b.InvoiceDate)
                .HasColumnName("InvoiceDate");
            builder
                .Property(b => b.DispatchDetail)
                .HasColumnName("DispatchDetail");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("DispatchDetails_TenantId");
        }
    }
}
