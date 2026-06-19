using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CWB.CommonUtils.Common.Configurations;
using CWB.Gro.Domain;

namespace CWB.Gro.Configurations
{
    public class Cust_Specific_DataConfiguration : IEntityTypeConfiguration<Cust_Specific_Data>
    {
        public void Configure(EntityTypeBuilder<Cust_Specific_Data> builder)
        {
            builder
                .ToTable("Cust_Specific_Data");
            builder
                .HasKey(w => w.Id);
            builder
              .Property(t => t.File_Location)
              .HasColumnName("File_Location")
              .IsRequired();
            builder
                .Property(t => t.Last_Upload_Row_No)
                .HasColumnName("Last_Upload_Row_No")
                .IsRequired();
            builder
                .Property(t => t.Last_Upload_date)
                .HasColumnName("Last_Upload_date")
                .HasDefaultValue(null);
            builder
                .Property(t => t.CWB_Customer)
                .HasColumnName("CWB_Customer")
                .IsRequired();
            builder
                .Property(t => t.UI_ID)
                .HasColumnName("UI_ID");
            builder
             .Property(t => t.Upload_Mapped_Table)
             .HasColumnName("Upload_Mapped_Table");
            builder
               .Property(t => t.Disp_Head_Map_Table)
               .HasColumnName("Disp_Head_Map_Table");
            builder
              .Property(t => t.Disp_Det_Map_Table)
              .HasColumnName("Disp_Det_Map_Table");
            
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Cust_Specific_Data_TenantId");
        }
    }
}
