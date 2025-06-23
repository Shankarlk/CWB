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
    public class Timeslot_ListConfiguration : IEntityTypeConfiguration<Timeslot_List>
    {
        public void Configure(EntityTypeBuilder<Timeslot_List> builder)
        {
            builder
                .ToTable("Timeslot_List");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.PlantId)
                .HasColumnName("PlantId");
            builder
                .Property(b => b.Start_time)
                .HasColumnName("Start_time");
            builder
                .Property(b => b.End_time)
                .HasColumnName("End_time");
            builder
                .Property(b => b.Break_Slot)
                .HasColumnName("Break_Slot");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Timeslot_List_TenantId");
        }
    }
}
