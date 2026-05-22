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
    public class Input_Resrv_ListConfiguration: IEntityTypeConfiguration<Input_Resrv_List>
    {
        public void Configure(EntityTypeBuilder<Input_Resrv_List> builder)
        {
            builder
                .ToTable("Input_Resrv_List");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.PO_NO_ID)
                .HasColumnName("PO_NO_ID");
            builder
                .Property(b => b.WO_Id)
                .HasColumnName("WO_Id");
            builder
                .Property(b => b.PartId)
                .HasColumnName("PartId");
            builder
                .Property(b => b.Plan_Alloc_Qnty)
                .HasColumnName("Plan_Alloc_Qnty");
            builder
                .Property(b => b.Allocation_done)
                .HasColumnName("Allocation_done");
            builder
                .Property(b => b.Bal_to_Issue)
                .HasColumnName("Bal_to_Issue");
            builder
                .Property(b => b.Qnty_Recd)
                .HasColumnName("Qnty_Recd");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
           
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Input_Resrv_List_TenantId");
        }
    }
}
