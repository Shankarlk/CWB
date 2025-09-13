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
    public class Role_UI_ListConfiguration : IEntityTypeConfiguration<Role_Ui_List>
    {
        public void Configure(EntityTypeBuilder<Role_Ui_List> builder)
        {
            builder
            .ToTable("Role_Ui_List");
            builder
               .HasKey(c => c.Id);
            builder
                .Property(t => t.Ui_Id)
                .HasColumnName("Ui_Id");
            builder
                .Property(t => t.RoleId)
                .HasColumnName("RoleId");
            builder
                .Property(t => t.EmployeeId)
                .HasColumnName("EmployeeId");
            builder
                .Property(t => t.Active)
                .HasColumnName("Active");
            builder
                .Property(t => t.Add_date)
                .HasColumnName("Add_date");
            builder
                .Property(t => t.Deact_date)
                .HasColumnName("Deact_date");
            builder
                .Property(t => t.PermissionId)
                .HasColumnName("PermissionId")
                .IsRequired();
            builder
                .Property(t => t.Comment)
                .HasColumnName("Comment");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Role_Ui_List_TenantId");
        }
    }
}
