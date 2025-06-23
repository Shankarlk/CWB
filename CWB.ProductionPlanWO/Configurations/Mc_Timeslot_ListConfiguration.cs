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
    public class Mc_Timeslot_ListConfiguration : IEntityTypeConfiguration<Mc_Timeslot_List>
    {
        public void Configure(EntityTypeBuilder<Mc_Timeslot_List> builder)
        {
            builder
                .ToTable("Mc_Timeslot_List");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.Timeslot_List_Id)
                .HasColumnName("Timeslot_List_Id");
            builder
                .Property(b => b.EndTimeslot_List_Id)
                .HasColumnName("EndTimeslot_List_Id");
            builder
                .Property(b => b.Mc_Id)
                .HasColumnName("Mc_Id");
            builder
                .Property(b => b.Mc_Wait_List_Id)
                .HasColumnName("Mc_Wait_List_Id");
            builder
                .Property(b => b.Allocation)
                .HasColumnName("Allocation");
            builder
                .Property(b => b.Slot_Not_Avl)
                .HasColumnName("Slot_Not_Avl");
            builder
                .Property(b => b.Not_Avl_reason)
                .HasColumnName("Not_Avl_reason");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Mc_Timeslot_List_TenantId");
        }
    }
}
