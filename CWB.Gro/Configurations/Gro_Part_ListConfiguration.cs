using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CWB.CommonUtils.Common.Configurations;
using CWB.Gro.Domain;

namespace CWB.Gro.Configurations
{
    public class Gro_Part_ListConfiguration: IEntityTypeConfiguration<Gro_Part_List>
    {
        public void Configure(EntityTypeBuilder<Gro_Part_List> builder)
        {
            builder
                .ToTable("Gro_Part_List");
            builder
                .HasKey(w=>w.Id);
            builder
              .Property(t => t.Gro_Part_No)
              .HasColumnName("Gro_Part_No")
              .IsRequired();
            builder
                .Property(t => t.Part_No)
                .HasColumnName("Part_No");
            builder
               .Property(t => t.MRP)
               .HasColumnName("MRP");
            builder
                .Property(t => t.Data_Update)
                .HasColumnName("Data_Update")
                .HasDefaultValue(null);
            builder
                .Property(t => t.Update_By)
                .HasColumnName("Update_By")
                .IsRequired();
            builder
                .Property(t => t.Part_Status)
                .HasColumnName("Part_Status")
                .IsRequired();
            builder
               .Property(t => t.OurPrice)
               .HasColumnName("OurPrice");
            builder
              .Property(t => t.GSTRate)
              .HasColumnName("GSTRate");
            builder
              .Property(t => t.HSNCode)
              .HasColumnName("HSNCode");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Gro_Part_List_TenantId");
        }
    }
}
