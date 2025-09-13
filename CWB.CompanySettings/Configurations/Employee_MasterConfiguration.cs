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
    public class Employee_MasterConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder
            .ToTable("Employee_Master");
            builder
               .HasKey(c => c.Id);
            builder
                .Property(t => t.Employee_name)
                .HasColumnName("Employee_name")
                .IsRequired();
            builder
                .Property(t => t.Designation_Id)
                .HasColumnName("Designation_Id")
                .IsRequired();
            builder
                .Property(t => t.Employee_No)
                .HasColumnName("Employee_No")
                .IsRequired();
            builder
                .Property(t => t.Date_Of_Joining)
                .HasColumnName("Date_Of_Joining")
                .IsRequired();
            builder
                .Property(t => t.Phone)
                .HasColumnName("Phone")
                .IsRequired();
            builder
                .Property(t => t.Email)
                .HasColumnName("Email")
                .IsRequired();
            builder
                .Property(t => t.UserName)
                .HasColumnName("UserName")
                .IsRequired();
            builder
                .Property(t => t.Password)
                .HasColumnName("Password")
                .IsRequired();
            builder
                .Property(t => t.Residential_Address)
                .HasColumnName("Residential_Address")
                .IsRequired();
            builder
                .Property(t => t.Emerg_Contact_Name)
                .HasColumnName("Emerg_Contact_Name");
            builder
                .Property(t => t.Emerg_Contact_No)
                .HasColumnName("Emerg_Contact_No");
            builder
                .Property(t => t.RoleIds)
                .HasColumnName("RoleIds");
            builder
                .Property(t => t.HeadOfDepartment)
                .HasColumnName("HeadOfDepartment");
            builder
                .Property(t => t.ChangedPassword)
                .HasColumnName("ChangedPassword");
            builder
                .Property(t => t.Plant_Id)
                .HasColumnName("Plant_Id")
                .IsRequired();
            builder
                .Property(t => t.RoleReportTo)
                .HasColumnName("RoleReportTo")
                .IsRequired();
            builder
                .Property(t => t.Home_Dept_Id)
                .HasColumnName("Home_Dept_Id")
                .IsRequired();
            builder
                .Property(t => t.Employee_Resigned)
                .HasColumnName("Employee_Resigned")
                .IsRequired();
            builder
                .Property(t => t.Date_Of_Resigning)
                .HasColumnName("Date_Of_Resigning");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Employee_Master_TenantId");
        }
    }
}
