using CWB.CommonUtils.Common.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CWB.Masters.Configurations
{
    public class RawMaterialDetailConfigurations : IEntityTypeConfiguration<Domain.RawMaterialDetail>
    {
        public void Configure(EntityTypeBuilder<Domain.RawMaterialDetail> builder)
        {
            builder
             .ToTable("RawMaterialDetails");
            builder
               .HasKey(m => m.Id);

            builder
                .Property(m => m.RawMaterialMadeType)
                .HasColumnName("RawMaterialMadeType")
                .HasMaxLength(255)
                .IsRequired();
            builder
                .Property(t => t.RawMaterialTypeId)
                .HasColumnName("RawMaterialTypeId")
                .IsUnicode(true)
                .HasMaxLength(255)
                .IsRequired();
            builder
                .Property(t => t.InHousePartNo)
                .HasColumnName("InHousePartNo")
                .IsUnicode(true)
                .HasMaxLength(255)
                .IsRequired();
            builder
                .Property(t => t.PartDescription)
                .HasConversion<string>()
                .HasColumnName("PartDescription")
                .IsUnicode(true)
                .HasMaxLength(255)
                .IsRequired();
            builder
                 .Property(t => t.BaseRawMaterialId)
                 .HasColumnName("BaseRawMaterialId")
                 .IsUnicode(true)
                 .HasMaxLength(255);
            builder
                 .Property(t => t.RawMaterialWeight)
                 .HasColumnName("RawMaterialWeight")
                 .IsUnicode(true)
                 .HasMaxLength(255);
            builder
                 .Property(t => t.RawMaterialNotes)
                 .HasConversion<string>()
                 .HasColumnName("RawMaterialNotes")
                 .IsUnicode(true)
                 .HasMaxLength(255);
            builder
                .Property(t => t.Status)
                .HasColumnName("Status")
                .IsUnicode(true)
                .HasMaxLength(255);
            builder
                .Property(t => t.StatusChangeReason)
                .HasConversion<string>()
                .HasColumnName("StatusChangeReason")
                .IsUnicode(true)
                .HasMaxLength(255);
            builder
                .Property(t => t.RevNo)
                .HasConversion<string>()
                .HasColumnName("RevNo")
                .IsUnicode(true)
                .HasMaxLength(255)
                .IsRequired();
            builder
                .Property(t => t.RevDate)
                .HasColumnName("RevDate")
                .IsUnicode(true)
                .HasMaxLength(255);
            builder
                .Property(t => t.Standard)
                .HasColumnName("Standard")
                .IsUnicode(true)
                .HasMaxLength(255);

            builder
                .Property(t => t.MaterialSpec)
                .HasConversion<string>()
                .HasColumnName("MaterialSpec")
                .IsUnicode(true)
                .HasMaxLength(255);
            builder
                .Property(t => t.PurchaseDetailId)
                .HasColumnName("PurchaseDetailId")
                .IsUnicode(true)
                .HasMaxLength(255);

            builder
                .Property(m => m.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(m => m.TenantId).HasDatabaseName("RawMaterialDetail_TenantId");
        }
    }
}
