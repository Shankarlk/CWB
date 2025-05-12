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
    public class NcLogStatusConfiguration : IEntityTypeConfiguration<NcLogStatus>
    {
        public void Configure(EntityTypeBuilder<NcLogStatus> builder)
        {
            builder
                .ToTable("NcLogStatus");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(c => c.Nc_Log_Status_Desc)
                .HasColumnName("Nc_Log_Status_Desc");
            builder.ConfigureBase();
            //builder.HasIndex(c => c.TenantId).HasDatabaseName("BOMList_TenantId");
        }
    }
}
