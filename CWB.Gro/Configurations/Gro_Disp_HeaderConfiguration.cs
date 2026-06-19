using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CWB.CommonUtils.Common.Configurations;
using CWB.Gro.Domain;

namespace CWB.Gro.Configurations
{
    public class Gro_Disp_HeaderConfiguration : IEntityTypeConfiguration<Gro_Disp_Header>
    {
        public void Configure(EntityTypeBuilder<Gro_Disp_Header> builder)
        {
            builder
                .ToTable("Gro_Disp_Header");
            builder
                .HasKey(w => w.Id);
            builder
              .Property(t => t.CWB_Customer)
              .HasColumnName("CWB_Customer")
              .IsRequired();
            builder
              .Property(t => t.Indent)
              .HasColumnName("Indent")
              .IsRequired();
            builder
                .Property(t => t.SentDate)
                .HasColumnName("SentDate")
                .HasDefaultValue(null);
           
            builder
              .Property(t => t.Shipping_Address)
              .HasColumnName("Shipping_Address");
            builder
              .Property(t => t.Shipping_City)
              .HasColumnName("Shipping_City")
              .IsRequired();
            builder
             .Property(t => t.Shipping_State)
             .HasColumnName("Shipping_State")
             .HasDefaultValue(0);
            builder
             .Property(t => t.Shipping_PINCODE)
             .HasColumnName("Shipping_PINCODE")
             .HasDefaultValue(0);
            builder
             .Property(t => t.Courier_Partner)
             .HasColumnName("Courier_Partner");
            builder
             .Property(t => t.Dispatch_Date)
             .HasColumnName("Dispatch_Date")
             .HasDefaultValue(null);
            builder
             .Property(t => t.Status)
             .HasColumnName("Status")
             .HasDefaultValue('N');
            builder
             .Property(t => t.AWB)
             .HasColumnName("AWB")
             .IsRequired();
            builder
             .Property(t => t.DC_Printed)
             .HasColumnName("DC_Printed")
             .HasDefaultValue('N');
            builder
               .Property(t => t.Delivered_Date)
               .HasColumnName("Delivered_Date")
               .HasDefaultValue(null);
            builder
           .Property(t => t.DC_No)
           .HasColumnName("DC_No")
           .IsRequired();
            builder
           .Property(t => t.Inv_No)
           .HasColumnName("Inv_No")
           .IsRequired();
            builder
            .Property(t => t.Inv_Printed)
            .HasColumnName("Inv_Printed")
            .HasDefaultValue('N');
            builder
           .Property(t => t.Inv_Uploaded)
           .HasColumnName("Inv_Uploaded")
           .HasDefaultValue('N');
            builder
          .Property(t => t.Customer_Inv_Attached)
          .HasColumnName("Customer_Inv_Attached")
          .HasDefaultValue('N');
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Gro_Disp_Header_TenantId");
        }
    }
}
