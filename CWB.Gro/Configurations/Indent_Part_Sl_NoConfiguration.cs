using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CWB.CommonUtils.Common.Configurations;
using CWB.Gro.Domain;

namespace CWB.Gro.Configurations
{
    public class Indent_Part_Sl_NoConfiguration : IEntityTypeConfiguration<Indent_Part_Sl_No>
    {
        public void Configure(EntityTypeBuilder<Indent_Part_Sl_No> builder)
        {
            builder
                .ToTable("Indent_Part_Sl_No");
            builder
                .HasKey(w => w.Id);
            builder
              .Property(t => t.Gro_Disp_Det_ID)
              .HasColumnName("Gro_Disp_Det_ID");
            builder
                .Property(t => t.Gro_Stock_Det_ID)
                .HasColumnName("Gro_Stock_Det_ID");
           


            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Indent_Part_Sl_No_TenantId");
        }
    }
}
