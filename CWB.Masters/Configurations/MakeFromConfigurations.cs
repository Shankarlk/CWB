using CWB.CommonUtils.Common.Configurations;
using CWB.Masters.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CWB.Masters.Configurations
{
    public class MakeFromConfigurations : IEntityTypeConfiguration<MPMakeFrom>
    {
        public void Configure(EntityTypeBuilder<MPMakeFrom> builder)
        {
            builder
            .ToTable("MPRawMeterials");
            builder
               .HasKey(c => c.Id);
            builder
                .Property(t => t.PartMadeFrom)
                .HasColumnName("PartMadeFrom")
                .IsUnicode(true)
                .HasMaxLength(255);
            builder
                .Property(t => t.InputWeight)
                .HasColumnName("InputWeight")
                .IsUnicode(true)
                .HasMaxLength(255);
            builder
                .Property(t => t.InputPartNo)
                .HasConversion<string>()
                .HasColumnName("InputPartNo")
                .IsUnicode(true)
                .HasMaxLength(255);
            builder
                .Property(t => t.Scrapgenerated)
                .HasColumnName("ScrapGenerated")
                .IsUnicode(true)
                .HasMaxLength(255);
            builder
                .Property(t => t.QuantityPerInput)
                .HasColumnName("QuantityPerInput")
                .IsUnicode(true)
                .HasMaxLength(255);
            builder
                .Property(t => t.YieldNotes)
                .HasConversion<string>()
                .HasColumnName("YieldNotes")
                .IsUnicode(true)
                .HasMaxLength(255);
            builder
                .Property(t => t.PreferedRawMaterial)
                .HasColumnName("PreferedRawMaterial")
                .IsUnicode(true)
                .HasMaxLength(255);
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("MPMakeFrom_TenantId");
        }
    }
}
