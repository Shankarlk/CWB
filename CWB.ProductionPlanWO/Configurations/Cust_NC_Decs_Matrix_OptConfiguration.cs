using System;
using System.Collections.Generic;
using System.Linq;
using CWB.CommonUtils.Common.Configurations;
using CWB.ProductionPlanWO.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Configurations
{
    public class Cust_NC_Decs_Matrix_OptConfiguration : IEntityTypeConfiguration<Cust_NC_Decs_Matrix_Opt>
    {
        public void Configure(EntityTypeBuilder<Cust_NC_Decs_Matrix_Opt> builder)
        {
            builder
                .ToTable("Cust_NC_Decs_Matrix_Opt");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.Cust_NC_Decs_Matrix_Id)
                .HasColumnName("Cust_NC_Decs_Matrix_Id");
            builder
                .Property(b => b.NC_Disp_Decision_Id)
                .HasColumnName("NC_Disp_Decision_Id");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Cust_NC_Decs_Matrix_Opt_TenantId");
        }
    }
}
