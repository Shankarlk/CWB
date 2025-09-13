using CWB.CommonUtils.Common.Configurations;
using CWB.CompanySettings.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.CompanySettings.Configurations
{
    public class Employee_PwdConfiguration : IEntityTypeConfiguration<Employee_Pwd>
    {
        public void Configure(EntityTypeBuilder<Employee_Pwd> builder)
        {

            builder
                  .ToTable("Employee_Pwd");
            builder.ConfigureBase();
            builder
                .Property(w => w.Employee_Id)
                .HasColumnName("Employee_Id");
            builder
                .Property(w => w.Date_Changed)
                .HasColumnName("Date_Changed");
            builder
               .Property(m => m.TenantId)
               .HasColumnName("TenantId");
            builder.HasIndex(m => m.TenantId).HasDatabaseName("Employee_Pwd_TenantId");
        }
    }
}
