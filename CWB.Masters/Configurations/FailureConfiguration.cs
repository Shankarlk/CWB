using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CWB.CommonUtils.Common.Configurations;
using CWB.Masters.Domain;
using CWB.Masters.Domain.DocumentManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CWB.Masters.Configurations
{
    public class FailureConfiguration : IEntityTypeConfiguration<Failure>
    {
        public void Configure(EntityTypeBuilder<Failure> builder)
        {
            builder
            .ToTable("Failure");
            builder
               .HasKey(c => c.Id);
            builder
                .Property(t => t.Message)
                .HasColumnName("Message");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Failure_TenantId");
        }
    }
}
