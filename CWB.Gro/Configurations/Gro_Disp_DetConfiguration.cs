using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CWB.CommonUtils.Common.Configurations;
using CWB.Gro.Domain;

namespace CWB.Gro.Configurations
{
    public class Gro_Disp_DetConfiguration : IEntityTypeConfiguration<Gro_Disp_Det>
    {
        public void Configure(EntityTypeBuilder<Gro_Disp_Det> builder)
        {
            builder
                .ToTable("Gro_Disp_Det");
            builder
                .HasKey(w => w.Id);
            builder
              .Property(t => t.Gro_Disp_Header_ID)
              .HasColumnName("Gro_Disp_Header_ID")
              .IsRequired();
            builder
              .Property(t => t.Gro_data_ID)
              .HasColumnName("Gro_data_ID")
              .IsRequired();
            builder
              .Property(t => t.Gro_Part_No)
              .HasColumnName("Gro_Part_No")
              .IsRequired();
            builder
              .Property(t => t.int_Part_No)
              .HasColumnName("int_Part_No")
              .IsRequired();
            builder
             .Property(t => t.Qnty_Dispatched)
             .HasColumnName("Qnty_Dispatched")
             .HasDefaultValue(0);
            builder
            .Property(t => t.Qnty_Recd)
            .HasColumnName("Qnty_Recd")
            .HasDefaultValue('N');
            builder
             .Property(t => t.Indent)
             .HasColumnName("Indent")
             .HasDefaultValue(0);
           
            builder
             .Property(t => t.Label_print)
             .HasColumnName("Label_print")
             .HasDefaultValue('N');
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Gro_Disp_Det_TenantId");
        }
    }
}
