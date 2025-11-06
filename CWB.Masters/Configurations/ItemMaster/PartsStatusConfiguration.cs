using CWB.CommonUtils.Common.Configurations;
using CWB.Masters.Domain.ItemMaster;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CWB.Masters.Configurations.ItemMaster
{
    public class PartsStatusConfiguration : IEntityTypeConfiguration<PartsStatus>
    {
        public void Configure(EntityTypeBuilder<PartsStatus> builder)
        {
            builder
             .ToTable("PartsStatuss");
            builder
               .HasKey(b => b.Id);
            builder
                .Property(b => b.Status)
                .HasColumnName("Status");
            builder
                .Property(b => b.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            //builder.ConfigureBase();
            builder.HasIndex(b => b.TenantId).HasDatabaseName("PartsStatus_TenantId");

            builder.HasData(
        new PartsStatus { Id = 1, Status = "Not Released" },
        new PartsStatus { Id = 2, Status = "Released" },
        new PartsStatus { Id = 3, Status = "Hold" },
        new PartsStatus { Id = 4, Status = "Obsolete" }
    );
        }
    }
}
