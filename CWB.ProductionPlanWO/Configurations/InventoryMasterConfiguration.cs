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
    public class Inventory_MasterConfiguration : IEntityTypeConfiguration<Inventory_Master>
    {
        public void Configure(EntityTypeBuilder<Inventory_Master> builder)
        {
            builder
                .ToTable("Inventory_Master");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.Dt_time)
                .HasColumnName("Dt_time");
            builder
                .Property(b => b.Part_NoId)
                .HasColumnName("Part_NoId");
            builder
                .Property(b => b.Routing_Id)
                .HasColumnName("Routing_Id");
            builder
                .Property(b => b.Opr_No_Id)
                .HasColumnName("Opr_No_Id");
            builder
                .Property(b => b.Current_QntOnHand)
                .HasColumnName("Current_QntOnHand");
            builder
                .Property(b => b.Location_Id)
                .HasColumnName("Location_Id");
            builder
                .Property(b => b.Inv_Trans_Log_Id)
                .HasColumnName("Inv_Trans_Log_Id");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Inventory_Master_TenantId");
        }
    }
}
