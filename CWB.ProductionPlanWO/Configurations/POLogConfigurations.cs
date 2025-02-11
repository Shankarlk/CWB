using CWB.CommonUtils.Common.Configurations;
using CWB.ProductionPlanWO.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CWB.ProductionPlanWO.Configurations
{
    public class POLogConfigurations : IEntityTypeConfiguration<POLog>
    {
        public void Configure(EntityTypeBuilder<POLog> builder)
        {
            builder
                .ToTable("POLog");
            builder
               .HasKey(c => c.Id);
            builder
                .Property(t => t.CustomerOrderId)
                .HasColumnName("WorkOrderId")
                .IsRequired();
            builder
                .Property(t => t.SalesOrderId)
                .HasColumnName("PoId")
                .IsRequired();
            builder
                .Property(t => t.PartId)
                .HasColumnName("PartId")
                .IsRequired();
            builder
               .Property(t => t.User)
               .HasColumnName("User")
               .HasMaxLength(255)
               .IsRequired();
            builder
                .Property(c => c.Event)
                .HasColumnName("Event");
            builder
                .Property(c => c.OldValue)
                .HasColumnName("OldValue");
            builder
                .Property(c => c.NewValue)
                .HasColumnName("NewValue");
            builder
               .Property(c => c.Comment)
               .HasColumnName("Comment");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("POLog_TenantId");
        }
    }
}
