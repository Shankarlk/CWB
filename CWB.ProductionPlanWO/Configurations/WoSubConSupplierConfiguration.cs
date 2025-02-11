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
    public class WoSubConSupplierConfiguration : IEntityTypeConfiguration<WoSubConSupplier>
    {
        public void Configure(EntityTypeBuilder<WoSubConSupplier> builder)
        {
            builder
                .ToTable("WoSubConSupplier");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.WoId)
                .HasColumnName("WoId");
            builder
                .Property(b => b.SubConId)
                .HasColumnName("ProcPlanId");
            builder
                .Property(b => b.SupplierId)
                .HasColumnName("SupplierId");
            builder
                .Property(b => b.Qnty)
                .HasColumnName("Qnty");
            builder
                .Property(b => b.DeliveryDate)
                .HasColumnName("DeliveryDate");
            builder
                .Property(b => b.ProcPrice)
                .HasColumnName("ProcPrice");
            builder
                .Property(b => b.AddnInfo)
                .HasColumnName("AddnInfo");
            builder
                .Property(b => b.RecieptDate)
                .HasColumnName("RecieptDate");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("WoSubConSupplier_TenantId");
        }
    }
}
