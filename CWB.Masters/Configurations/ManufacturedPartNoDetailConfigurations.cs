using CWB.CommonUtils.Common.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CWB.Masters.Configurations
{
    public class ManufacturedPartNoDetailConfigurations : IEntityTypeConfiguration<Domain.ManufacturedPartNoDetail>
    {
        public void Configure(EntityTypeBuilder<Domain.ManufacturedPartNoDetail> builder)
        {
            builder
             .ToTable("ManufacturedPartNoDetails");
            builder
               .HasKey(m => m.Id);

            builder
                .Property(m => m.ManufacturedPartType)
                .HasColumnName("ManufacturedPartType")
                .HasMaxLength(255)
                .IsRequired();
            builder
                .Property(t => t.CompanyName)
                .HasConversion<string>()
                .HasColumnName("CompanyName")
                .IsUnicode(true)
                .HasMaxLength(255)
                .IsRequired();
            builder
                .Property(t => t.PartNumber)
                .HasConversion<string>()
                .HasColumnName("PartNumber")
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
                .Property(t => t.FinishedWeight)
                .HasColumnName("FinishedWeight")
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
                .Property(t => t.UOMId)
                .HasColumnName("UOMId")
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
                .Property(t => t.MakeFrom)
                .HasConversion<string>()
                .HasColumnName("MakeFrom")
                .IsUnicode(true)
                .HasMaxLength(255);
            builder
                .Property(t => t.BOM)
                .HasConversion<string>()
                .HasColumnName("BOM")
                .IsUnicode(true)
                .HasMaxLength(255);

            builder
                .Property(m => m.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(m => m.TenantId).HasDatabaseName("ManufacturedPartNoDetail_TenantId");
        }
    }
}
