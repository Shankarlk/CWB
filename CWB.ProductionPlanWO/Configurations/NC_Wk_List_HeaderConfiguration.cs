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
    public class NC_Wk_List_HeaderConfiguration : IEntityTypeConfiguration<NC_Wk_List_Header>
    {
        public void Configure(EntityTypeBuilder<NC_Wk_List_Header> builder)
        {
            builder
                .ToTable("NC_Wk_List_Header");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.NC_Log_Id)
                .HasColumnName("NC_Log_Id");
            builder
                .Property(b => b.NC_Wk_List_Header_Status)
                .HasColumnName("NC_Wk_List_Header_Status");
            builder
                .Property(b => b.Start_Date)
                .HasColumnName("Start_Date");
            builder
                .Property(b => b.End_Date)
                .HasColumnName("End_Date");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("NC_Wk_List_Header_TenantId");
        }
    }
}
