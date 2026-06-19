using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CWB.CommonUtils.Common.Configurations;
using CWB.Gro.Domain;

namespace CWB.Gro.Configurations
{
    public class TK_DC_Inv_ContrlConfiguration : IEntityTypeConfiguration<TK_DC_Inv_Contrl>
    {
        public void Configure(EntityTypeBuilder<TK_DC_Inv_Contrl> builder)
        {
            builder
                .ToTable("TK_DC_Inv_Contrl");
            builder
                .HasKey(w => w.Id);
            builder
              .Property(t => t.TK_DC_Last_No)
              .HasColumnName("TK_DC_Last_No");
            builder
                .Property(t => t.TK_Inv_Last_No)
                .HasColumnName("TK_Inv_Last_No");
            builder
                .Property(t => t.DC_Enable)
                .HasColumnName("DC_Enable").
                HasDefaultValue('N');
            builder
               .Property(t => t.Inv_Print_Enable)
               .HasColumnName("Inv_Print_Enable").
               HasDefaultValue('N');
            builder
               .Property(t => t.Inv_Push_Enable)
               .HasColumnName("Inv_Push_Enable").
               HasDefaultValue('N');


            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("TK_DC_Inv_Contrl_TenantId");
        }
    }
}
