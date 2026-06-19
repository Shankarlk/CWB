using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CWB.CommonUtils.Common.Configurations;
using CWB.Gro.Domain;

namespace CWB.Gro.Configurations
{
    public class Gro_Stock_DetConfiguration : IEntityTypeConfiguration<Gro_Stock_Det>
    {
        public void Configure(EntityTypeBuilder<Gro_Stock_Det> builder)
        {
            builder
                .ToTable("Gro_Stock_Det");
            builder
                .HasKey(w => w.Id);
            builder
              .Property(t => t.Gro_Part_List_ID)
              .HasColumnName("Gro_Part_List_ID");
            builder
                .Property(t => t.Part_Sl_No)
                .HasColumnName("Part_Sl_No");
            builder
                .Property(t => t.Sl_No_Status_ID)
                .HasColumnName("Sl_No_Status_ID");


            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Courier_List_TenantId");
        }
    }
}
