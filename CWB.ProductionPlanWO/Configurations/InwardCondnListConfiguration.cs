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
    public class InwardCondnListConfiguration : IEntityTypeConfiguration<Inward_Condn_list>
    {
        public void Configure(EntityTypeBuilder<Inward_Condn_list> builder)
        {
            builder
                .ToTable("Inward_Condn_list");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.Inward_Condn_desc)
                .HasColumnName("Inward_Condn_desc");
            builder
                .Property(b => b.Applicability)
                .HasColumnName("Applicability");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Inward_Condn_list_TenantId");
        }
    }
}
