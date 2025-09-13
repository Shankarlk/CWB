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
    public class Dept_EmployeeConfiguration : IEntityTypeConfiguration<Dept_Employee>
    {
        public void Configure(EntityTypeBuilder<Dept_Employee> builder)
        {

            builder
                  .ToTable("Dept_Employee");
            builder.ConfigureBase();
            builder
                .Property(w => w.Employee_Id)
                .HasColumnName("Employee_Id");
            builder
                .Property(w => w.Dept_Posn)
                .HasColumnName("Dept_Posn");
            builder
                .Property(w => w.Deact_date)
                .HasColumnName("Deact_date");
            builder
                .Property(w => w.Add_date)
                .HasColumnName("Add_date");
            builder
                .Property(w => w.Active)
                .HasColumnName("Active");
            builder
               .Property(m => m.TenantId)
               .HasColumnName("TenantId");
            builder.HasIndex(m => m.TenantId).HasDatabaseName("Dept_Employee_TenantId");
        }
    }
}
