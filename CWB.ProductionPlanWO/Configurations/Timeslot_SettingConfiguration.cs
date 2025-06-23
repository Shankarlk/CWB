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
    public class Timeslot_SettingConfiguration : IEntityTypeConfiguration<Timeslot_Setting>
    {
        public void Configure(EntityTypeBuilder<Timeslot_Setting> builder)
        {
            builder
                .ToTable("Timeslot_Setting");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.Timeslot_duration)
                .HasColumnName("Timeslot_duration");
            builder
                .Property(b => b.No_of_span_days)
                .HasColumnName("No_of_span_days");
            builder
                .Property(b => b.Retention_Days)
                .HasColumnName("Retention_Days");
            builder
                .Property(b => b.First_Shift_Break_start_time)
                .HasColumnName("First_Shift_Break_start_time");
            builder
                .Property(b => b.First_Shift_Break_duration)
                .HasColumnName("First_Shift_Break_duration");
            builder
                .Property(b => b.Sec_Shift_Break_start_time)
                .HasColumnName("Sec_Shift_Break_start_time");
            builder
                .Property(b => b.Sec_Shift_Break_duration)
                .HasColumnName("Sec_Shift_Break_duration");
            builder
                .Property(b => b.Third_Shift_Break_start_time)
                .HasColumnName("Third_Shift_Break_start_time");
            builder
                .Property(b => b.Third_Shift_Break_duration)
                .HasColumnName("Third_Shift_Break_duration");
            builder
                .Property(b => b.Change_flag)
                .HasColumnName("Change_flag");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Timeslot_Setting_TenantId");
        }
    }
}
