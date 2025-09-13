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
    public class Employee_UI_ListConfiguration : IEntityTypeConfiguration<Employee_UI_List>
    {
        public void Configure(EntityTypeBuilder<Employee_UI_List> builder)
        {

            builder
                  .ToTable("Employee_UI_List");
            builder.ConfigureBase();
            builder
                .Property(w => w.Ui_Id)
                .HasColumnName("Ui_Id");
            builder
                .Property(w => w.Employee_Id)
                .HasColumnName("Employee_Id");
            builder
                .Property(w => w.Access_Level)
                .HasColumnName("Access_Level");
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
            builder.HasIndex(m => m.TenantId).HasDatabaseName("Employee_UI_List_TenantId");
        }
    }
}
