using CWB.BusinessAquisition.Domain;
using CWB.CommonUtils.Common.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CWB.BusinessAquisition.Configurations
{
    public class SO_Alloc_ListConfiguration : IEntityTypeConfiguration<SO_Alloc_List>
    {
        public void Configure(EntityTypeBuilder<SO_Alloc_List> builder)
        {
            builder
            .ToTable("SO_Alloc_List");
            builder
               .HasKey(c => c.Id);
            builder
                .Property(t => t.SO_ID)
                .HasColumnName("SO_ID")
                .IsRequired();
            builder
                .Property(t => t.PartId)
                .HasColumnName("PartId")
                .IsRequired();
            /*builder
                .Property(t => t.ScheduleId)
                .HasColumnName("ScheduleId")
                .IsRequired();*/
            builder
                .Property(t => t.Allocation_Date)
                .HasColumnName("Allocation_Date")
                .IsRequired();
            builder
                .Property(t => t.Allocated_Qnty)
                .HasColumnName("Allocated_Qnty")
                .IsRequired();
            builder
              .Property(t => t.Dispatch_Complete)
              .HasColumnName("Dispatch_Complete");
             
            builder
             .Property(t => t.Final_Dispatch_Qnty)
             .HasColumnName("Final_Dispatch_Qnty")
             .HasDefaultValue(0);
           
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("SO_Alloc_List_TenantId");
        }
    }
}
