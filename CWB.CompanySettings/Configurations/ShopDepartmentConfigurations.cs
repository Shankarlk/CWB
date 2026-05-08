using CWB.CommonUtils.Common.Configurations;
using CWB.CompanySettings.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CWB.CompanySettings.Configurations
{
    public class ShopDepartmentConfigurations : IEntityTypeConfiguration<ShopDepartment>
    {
        public void Configure(EntityTypeBuilder<ShopDepartment> builder)
        {

            builder
                  .ToTable("ShopDepartments");
            builder.ConfigureBase();
            builder
                .Property(w => w.Name)
                .HasColumnName("Name")
                .IsUnicode(true)
                .HasMaxLength(255)
                .IsRequired();
            builder
                .Property(w => w.NoOfShifts)
                .HasColumnName("NoOfShifts")
                .IsRequired()
                .HasDefaultValue(1);
            builder
               .HasOne(m => m.Plant)
               .WithMany(m => m.ShopDepartments)
               .HasForeignKey(m => m.PlantId)
               .IsRequired();
            builder
               .Property(m => m.TenantId)
               .HasColumnName("TenantId")
               .IsRequired();
            builder
               .Property(m => m.Part_Of)
               .HasColumnName("Part_Of")
               .IsRequired();
            builder
               .Property(m => m.Level_No)
               .HasColumnName("Level_No")
               .IsRequired();
            builder
                .Property(t => t.Activity)
                .HasColumnName("Activity")
                .IsUnicode(true)
                .HasMaxLength(4000)
                .HasColumnType("nvarchar(MAX)");
            builder
               .Property(t => t.ProdDept)
               .HasColumnName("ProdDept")
               .IsRequired(); 
            builder
              .Property(t => t.Section)
              .HasColumnName("Section")
              .HasMaxLength(255);
            builder
             .Property(t => t.Stores_DirectMatl)
             .HasColumnName("Stores_DirectMatl")
             .HasDefaultValue(0);
            builder
            .Property(t => t.Stores_Cust_Dispatch)
            .HasColumnName("Stores_Cust_Dispatch")
            .HasDefaultValue(0);
            builder
            .Property(t => t.Stores_Tools)
            .HasColumnName("Stores_Tools")
            .HasDefaultValue(0);
            builder
            .Property(t => t.Stores_Consumables)
            .HasColumnName("Stores_Consumables")
            .HasDefaultValue(0);
            builder.HasIndex(m => m.PlantId).HasDatabaseName("ShopDepartment_PlantId");
            builder.HasIndex(m => m.TenantId).HasDatabaseName("ShopDepartment_TenantId");
        }
    }
}
