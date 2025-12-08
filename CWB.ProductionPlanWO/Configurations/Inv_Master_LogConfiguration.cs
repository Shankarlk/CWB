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
    public class Inv_Master_LogConfiguration : IEntityTypeConfiguration<Inv_Master_Log>
    {
        public void Configure(EntityTypeBuilder<Inv_Master_Log> builder)
        {
            builder
                .ToTable("Inv_Master_Log");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.Inv_mast_ID)
                .HasColumnName("Inv_mast_ID");
            builder
                .Property(b => b.Dt_time)
                .HasColumnName("Dt_time");
            builder
                .Property(b => b.Part_No)
                .HasColumnName("Part_No");
            builder
                .Property(b => b.Changed_by)
                .HasColumnName("Changed_by");
            builder
                .Property(b => b.Field_Changed)
                .HasColumnName("Field_Changed");
            builder
                .Property(b => b.Old_Value)
                .HasColumnName("Old_Value");
            builder
                .Property(b => b.New_Value)
                .HasColumnName("New_Value");
            builder
                .Property(b => b.Reason_Desc)
                .HasColumnName("Reason_Desc");
            builder
                .Property(b => b.Financial_Impact)
                .HasColumnName("Financial_Impact");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Inv_Master_Log_TenantId");
        }
    }
}
