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
    public class Cont_RCA_CA_LogConfiguration : IEntityTypeConfiguration<Cont_RCA_CA_Log>
    {
        public void Configure(EntityTypeBuilder<Cont_RCA_CA_Log> builder)
        {
            builder
                .ToTable("Cont_RCA_CA_Log");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.NcLogId)
                .HasColumnName("NcLogId");
            builder
                .Property(b => b.Containment_Action)
                .HasColumnName("Containment_Action");
            builder
                .Property(b => b.Cont_RCA_CA_Status_Id)
                .HasColumnName("Cont_RCA_CA_Status_Id");
            builder
                .Property(b => b.Senior_Feedback)
                .HasColumnName("Senior_Feedback");
            builder
                .Property(b => b.CnfAllCompelete)
                .HasColumnName("CnfAllCompelete");
            builder
                .Property(b => b.CnfComments)
                .HasColumnName("CnfComments");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Cont_RCA_CA_Log_TenantId");
        }
    }
}
