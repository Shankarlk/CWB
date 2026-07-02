using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CWB.CommonUtils.Common.Configurations;
using CWB.Gro.Domain;
namespace CWB.Gro.Configurations
{
    public class Gro_Indent_DispHeadConfiguration : IEntityTypeConfiguration<Gro_Indent_DispHead>
    {
        public void Configure(EntityTypeBuilder<Gro_Indent_DispHead> builder)
        {
            builder
                .ToTable("Gro_Indent_DispHead");
            builder
                .HasKey(w => w.Id);
            builder
              .Property(t => t.Gro_Indent)
              .HasColumnName("Gro_Indent");
            builder
                .Property(t => t.Gro_Disp_Header_ID)
                .HasColumnName("Gro_Disp_Header_ID");



            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Gro_Indent_DispHead_TenantId");
        }
    }
}
