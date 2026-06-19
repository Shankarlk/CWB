using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CWB.CommonUtils.Common.Configurations;
using CWB.Gro.Domain;


namespace CWB.Gro.Configurations
{
    public class Gro_Stock_ListConfiguration:IEntityTypeConfiguration<Gro_Stock_List>
    {
        public void Configure(EntityTypeBuilder<Gro_Stock_List> builder)
        {
            builder
                .ToTable("Gro_Stock_List");
            builder
                .HasKey(w => w.Id);
            builder
              .Property(t => t.Gro_Part_List_ID)
              .HasColumnName("Gro_Part_List_ID")
              .IsRequired();
            builder
              .Property(t => t.Qnty_on_Hand)
              .HasColumnName("Qnty_on_Hand");
            builder
              .Property(t => t.Last_Sl_No)
              .HasColumnName("Last_Sl_No")
              .IsRequired();
            builder
                .Property(t => t.Qnty_Correction_Date)
                .HasColumnName("Qnty_Correction_Date")
                .HasDefaultValue(null);

            builder
              .Property(t => t.Correction_User)
              .HasColumnName("Correction_User");
           
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Gro_Stock_List_TenantId");
        }
    }
}
