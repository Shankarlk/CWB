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
    public class Inw_Recpt_Part_NoConfiguration : IEntityTypeConfiguration<Inw_Recpt_Part_No>
    {
        public void Configure(EntityTypeBuilder<Inw_Recpt_Part_No> builder)
        {
            builder
                .ToTable("Inw_Recpt_Part_No");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.PO_Details_Id)
                .HasColumnName("PO_Details_Id");
            builder
                .Property(b => b.Release_Insp)
                .HasColumnName("Release_Insp");
            builder
                .Property(b => b.Insp_Complete)
                .HasColumnName("Insp_Complete");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Inw_Recpt_Part_No_TenantId");
        }
    }
}
