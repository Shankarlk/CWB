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
    public class Matl_Issue_SettingsConfiguration : IEntityTypeConfiguration<Matl_Issue_Settings>
    {
        public void Configure(EntityTypeBuilder<Matl_Issue_Settings> builder)
        {
            builder
                .ToTable("Matl_Issue_Settings");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.Shop_Id)
                .HasColumnName("Shop_Id");
            builder
                .Property(b => b.IssueDay)
                .HasColumnName("IssueDay");
            builder
                .Property(b => b.No_days_coverage)
                .HasColumnName("No_days_coverage");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Matl_Issue_Settings_TenantId");
        }
    }
}
