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
    public class Dept_Role_ListConfiguration : IEntityTypeConfiguration<Dept_Role_List>
    {
        public void Configure(EntityTypeBuilder<Dept_Role_List> builder)
        {

            builder
                  .ToTable("Dept_Role_List");
            builder.ConfigureBase();
            builder
                .Property(w => w.Dept_Struct_Id)
                .HasColumnName("Dept_Struct_Id");
            builder
                .Property(w => w.Role_Access_Id)
                .HasColumnName("Role_Access_Id");
            builder
               .Property(m => m.TenantId)
               .HasColumnName("TenantId");
            builder.HasIndex(m => m.TenantId).HasDatabaseName("Dept_Role_List_TenantId");
        }
    }
}
