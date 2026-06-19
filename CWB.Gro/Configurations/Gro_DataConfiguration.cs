using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CWB.CommonUtils.Common.Configurations;
using CWB.Gro.Domain;
namespace CWB.Gro.Configurations
{
    public class Gro_DataConfiguration:IEntityTypeConfiguration<Gro_Data>
    {
        public void Configure(EntityTypeBuilder<Gro_Data> builder)
        {
            builder
                .ToTable("Gro_Data");
            builder
                .HasKey(w => w.Id);
            builder
              .Property(t => t.CWB_Customer)
              .HasColumnName("CWB_Customer")
              .IsRequired();
            builder
                .Property(t => t.int_Part_No)
                .HasColumnName("int_Part_No");
            builder
                .Property(t => t.SentDate)
                .HasColumnName("SentDate")
                .HasDefaultValue(null);
            builder
                .Property(t => t.Excutive_Name)
                .HasColumnName("Excutive_Name")
                .IsRequired();
            builder
                .Property(t => t.Indent)
                .HasColumnName("Indent")
                .IsRequired();
            builder
             .Property(t => t.Company_Name)
             .HasColumnName("Company_Name")
             .IsRequired();
            builder
               .Property(t => t.Gro_Part_No)
               .HasColumnName("Gro_Part_No")
               .IsRequired();
            builder
              .Property(t => t.Reqd_Quantity)
              .HasColumnName("Reqd_Quantity")
              .IsRequired();
            builder
              .Property(t => t.Remarks)
              .HasColumnName("Remarks")
              .HasDefaultValue(0);
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
             .Property(t => t.Contact_Person)
             .HasColumnName("Contact_Person");
            builder
             .Property(t => t.Contact_Person_No)
             .HasColumnName("Contact_Person_No");
            builder
             .Property(t => t.Part_Not_Avl)
             .HasColumnName("Part_Not_Avl")
             .HasDefaultValue('N');
            builder
             .Property(t => t.Bal_to_Disp)
             .HasColumnName("Bal_to_Disp")
             .IsRequired();
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Gro_Data_TenantId");
        }
    }
}
