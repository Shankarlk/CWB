using CWB.BusinessAquisition.Domain;
using CWB.CommonUtils.Common.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CWB.BusinessAquisition.Configurations
{
    public class Cust_Disp_ListConfiguration : IEntityTypeConfiguration<Cust_Disp_List>
    {
        public void Configure(EntityTypeBuilder<Cust_Disp_List> builder)
        {
            builder
            .ToTable("Cust_Disp_List");
            builder
               .HasKey(c => c.Id);
            builder
                .Property(t => t.SO_ID)
                .HasColumnName("SO_ID")
                .IsRequired();
            builder
                .Property(t => t.PartId)
                .HasColumnName("PartId")
                .IsRequired();
            builder
                .Property(t => t.Disp_Qnty)
                .HasColumnName("Disp_Qnty")
                .IsRequired();
            
            builder
              .Property(t => t.Dispatch_Complete)
              .HasColumnName("Dispatch_Complete");
            builder
                .Property(t => t.Invoice_Details_Entered)
                .HasColumnName("Invoice_Details_Entered");
            builder
             .Property(t => t.Data_Confirmed)
             .HasColumnName("Data_Confirmed");
            

            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Cust_Disp_List_TenantId");
        }
    }
}
