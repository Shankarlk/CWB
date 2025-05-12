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
    public class NC_Decision_LogConfiguration : IEntityTypeConfiguration<NC_Decision_Log>
    {
        public void Configure(EntityTypeBuilder<NC_Decision_Log> builder)
        {
            builder
                .ToTable("NC_Decision_Log");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.NcLogId)
                .HasColumnName("NcLogId");
            builder
                .Property(b => b.RedoCA)
                .HasColumnName("RedoCA");
            builder
                .Property(b => b.Cust_Decision_Reqd)
                .HasColumnName("Cust_Decision_Reqd");
            builder
                .Property(b => b.Cust_Dec_Request)
                .HasColumnName("Cust_Dec_Request");
            builder
                .Property(b => b.Cust_Request_date)
                .HasColumnName("Cust_Request_date");
            builder
                .Property(b => b.Cust_feedback_date)
                .HasColumnName("Cust_feedback_date");
            builder
                .Property(b => b.Cust_feedback_Id)
                .HasColumnName("Cust_feedback_Id");
            builder
                .Property(b => b.Cust_Feedback_Desc)
                .HasColumnName("Cust_Feedback_Desc");
            builder
                .Property(b => b.NC_Disp_Decision_Id)
                .HasColumnName("NC_Disp_Decision_Id");
            builder
                .Property(b => b.NC_Disp_Instruction)
                .HasColumnName("NC_Disp_Instruction");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("NC_Decision_Log_TenantId");
        }
    }
}
