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
    public class TempDispatchQntyConfiguration : IEntityTypeConfiguration<TempDispatchQnty>
    {
        public void Configure(EntityTypeBuilder<TempDispatchQnty> builder)
        {
            builder
                .ToTable("TempDispatchQnty");
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
                .Property(b => b.InventoryMasterId)
                .HasColumnName("InventoryMasterId");
            builder
                .Property(b => b.QntyToDispatch)
                .HasColumnName("QntyToDispatch");
            builder
                .Property(b => b.SuggestedQnty)
                .HasColumnName("SuggestedQnty");
            builder
                .Property(b => b.FinalDispQnty)
                .HasColumnName("FinalDispQnty");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("DispatchQnty_TenantId");
        }
    }
}
