using CWB.BusinessAquisition.Domain;
using CWB.CommonUtils.Common.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CWB.BusinessAquisition.Configurations
{
    public class Invoice_Disp_ListConfiguration:IEntityTypeConfiguration<Invoice_Disp_List>
    {
        public void Configure(EntityTypeBuilder<Invoice_Disp_List> builder)
        {
            builder
            .ToTable("Invoice_Disp_List");
            builder
               .HasKey(c => c.Id);
            builder
                .Property(t => t.Dispatch_Detail_Id)
                .HasColumnName("Dispatch_Detail_Id")
                .IsRequired();
            builder
                .Property(t => t.Cust_Disp_List_Id)
                .HasColumnName("Cust_Disp_List_Id")
                .IsRequired();
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Invoice_Disp_List_TenantId");
        }
    }
}
