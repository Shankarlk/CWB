using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CWB.CommonUtils.Common.Configurations;
using CWB.Gro.Domain;


namespace CWB.Gro.Configurations
{
    public class Upload_FormatConfiguration : IEntityTypeConfiguration<Upload_Format>
    {
        public void Configure(EntityTypeBuilder<Upload_Format> builder)
        {
            builder
                .ToTable("Upload_Format");
            builder
                .HasKey(w => w.Id);
            builder
              .Property(t => t.CWB_Customer)
              .HasColumnName("CWB_Customer")
              .IsRequired();
            builder
                .Property(t => t.Column_Name)
                .HasColumnName("Column_Name")
                .IsRequired();
            builder
            .Property(t => t.Column_Position)
            .HasColumnName("Column_Position")
            .IsRequired();
            builder
                .Property(t => t.Field_Type)
                .HasColumnName("Field_Type")
                .IsRequired();
        
            builder
                .Property(t => t.Mapped_Field)
                .HasColumnName("Mapped_Field");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Upload_Format_TenantId");
        }
    }
}
