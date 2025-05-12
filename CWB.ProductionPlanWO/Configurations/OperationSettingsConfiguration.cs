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
    public class OperationSettingsConfiguration : IEntityTypeConfiguration<OperationSettings>
    {
        public void Configure(EntityTypeBuilder<OperationSettings> builder)
        {
            builder
                .ToTable("OperationSettings");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(c => c.UiName)
                .HasColumnName("UiName");
            builder
                .Property(c => c.EnableDisable)
                .HasColumnName("EnableDisable")
                .HasDefaultValue('Y');
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("OperationSettings_TenantId");

            builder.HasData(
        new OperationSettings { Id = 1, UiName = "Enable Upload of Inwarding Documents ",EnableDisable= 'Y' },
        new OperationSettings { Id = 2, UiName = "Enable Upload of Supplier Inspection Documents",EnableDisable='Y'},
        new OperationSettings { Id = 3, UiName = "Enable Printing of Inwarding Labels",EnableDisable='Y'},
        new OperationSettings { Id = 4, UiName = "Enable Viewing of Part Inspection Documents",EnableDisable='Y'},
        new OperationSettings { Id = 5, UiName = "Enable Viewing of Supplier Inspection Documents",EnableDisable='Y'},
        new OperationSettings { Id = 6, UiName = "Enable Upload of Our Inward Inspection Documents",EnableDisable='Y'},
        new OperationSettings { Id = 7, UiName = "Enable Upload of Root Cause Analysis & Corrective Action Report",EnableDisable='Y'},
        new OperationSettings { Id = 8, UiName = "Enable Printing of Inspection Labels",EnableDisable='Y'},
        new OperationSettings { Id = 9, UiName = "Allow User with Permission to open NC Disposal UI without RCA & CA uploaded",EnableDisable='Y'}
    );
        }
    }
}
