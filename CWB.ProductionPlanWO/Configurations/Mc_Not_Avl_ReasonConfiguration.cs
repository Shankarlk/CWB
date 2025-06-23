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
    public class Mc_Not_Avl_ReasonConfiguration : IEntityTypeConfiguration<Mc_Not_Avl_Reason>
    {
        public void Configure(EntityTypeBuilder<Mc_Not_Avl_Reason> builder)
        {
            builder
                .ToTable("Mc_Not_Avl_Reason");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.Reason_Desc)
                .HasColumnName("Reason_Desc");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Mc_Not_Avl_Reason_TenantId");
        }
    }
}
