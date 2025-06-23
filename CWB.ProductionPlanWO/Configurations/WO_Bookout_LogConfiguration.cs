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
    public class WO_Bookout_LogConfiguration : IEntityTypeConfiguration<WO_Bookout_Log>
    {
        public void Configure(EntityTypeBuilder<WO_Bookout_Log> builder)
        {
            builder
                .ToTable("WO_Bookout_Log");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.Wo_Id)
                .HasColumnName("Wo_Id");
            builder
                .Property(b => b.Opr_No)
                .HasColumnName("Opr_No");
            builder
                .Property(b => b.Event_date_time)
                .HasColumnName("Event_date_time");
            builder
                .Property(b => b.Bookout_Qnty)
                .HasColumnName("Bookout_Qnty");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("WO_Bookout_Log_TenantId");
        }
    }
}
