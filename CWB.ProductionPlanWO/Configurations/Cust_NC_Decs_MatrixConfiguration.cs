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
    public class Cust_NC_Decs_MatrixConfiguration : IEntityTypeConfiguration<Cust_NC_Decs_Matrix>
    {
        public void Configure(EntityTypeBuilder<Cust_NC_Decs_Matrix> builder)
        {
            builder
                .ToTable("Cust_NC_Decs_Matrix");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.Cust_Request_Id)
                .HasColumnName("Cust_Request_Id");
            builder
                .Property(b => b.Cust_Feedback_Id)
                .HasColumnName("Cust_Feedback_Id");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Cust_NC_Decs_Matrix_TenantId");
        }
    }
}
