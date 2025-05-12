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
    public class NC_Work_ListConfiguration : IEntityTypeConfiguration<NC_Work_List>
    {
        public void Configure(EntityTypeBuilder<NC_Work_List> builder)
        {
            builder
                .ToTable("NC_Work_List");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.NC_Work_List_Header_Id)
                .HasColumnName("NC_Work_List_Header_Id");
            builder
                .Property(b => b.Step_Desc)
                .HasColumnName("Step_Desc");
            builder
                .Property(b => b.Resp_Dept)
                .HasColumnName("Resp_Dept");
            builder
                .Property(b => b.Seq_No)
                .HasColumnName("Seq_No");
            builder
                .Property(b => b.NC_Work_status_Id)
                .HasColumnName("NC_Work_status_Id");
            builder
                .Property(b => b.Start_Date)
                .HasColumnName("Start_Date");
            builder
                .Property(b => b.End_Date)
                .HasColumnName("End_Date");
            builder
                .Property(b => b.Resp_Person)
                .HasColumnName("Resp_Person");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("NC_Work_List_TenantId");
        }
    }
}
