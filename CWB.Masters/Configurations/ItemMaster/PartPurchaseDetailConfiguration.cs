using CWB.CommonUtils.Common.Configurations;
using CWB.Masters.Domain.ItemMaster;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CWB.Masters.Configurations.ItemMaster
{
    public class PartPurchaseDetailConfiguration : IEntityTypeConfiguration<PartPurchaseDetail>
    {
        public void Configure(EntityTypeBuilder<PartPurchaseDetail> builder)
        {
            builder
             .ToTable("PartPurchaseDetails");
            builder
               .HasKey(b => b.Id);
            builder
                .HasOne(b => b.MasterPart)
                .WithMany(b => b.PartPurchaseDetails)
                .HasForeignKey(b => b.MasterPartId)
                .IsRequired();
            builder
                .HasOne(m => m.Company)
                .WithMany(m => m.PartPurchaseDetails)
                .HasForeignKey(m => m.SupplierId)
                .IsRequired();
            builder
                .Property(b => b.SupplierPartNo)
                .HasColumnName("SupplierPartNo")
                .HasMaxLength(255)
                .IsRequired();
            builder
               .Property(b => b.LeadTimeInDays)
               .HasColumnName("LeadTimeInDays")
               .IsRequired();
            builder
               .Property(b => b.MinimumOrderQuantity)
               .HasColumnName("MinimumOrderQuantity")
               .IsRequired();
            builder
               .Property(b => b.Price)
               .HasColumnName("Price")
               .HasMaxLength(20)
               .IsRequired();
            builder
                .Property(b => b.ShareOfBusiness)
                .HasColumnName("ShareOfBusiness")
                .HasMaxLength(255)
                .IsRequired();
            builder
                .Property(b => b.AdditionalInformation)
                .HasColumnName("AdditionalInformation")
                .IsUnicode(true)
                .HasMaxLength(4000)
                .HasColumnType("nvarchar(MAX)");
            builder
               .Property(b => b.TenantId)
               .HasColumnName("TenantId")
               .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(b => b.TenantId).HasDatabaseName("PartPurchaseDetail_TenantId");
            builder.HasIndex(b => b.MasterPartId).HasDatabaseName("PartPurchaseDetail_MasterPartId");
        }
    }
}
