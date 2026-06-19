using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CWB.CommonUtils.Common.Configurations;
using CWB.Gro.Domain;

namespace CWB.Gro.Configurations
{
    public class Courier_ListConfiguration : IEntityTypeConfiguration<Courier_List>
    {
        public void Configure(EntityTypeBuilder<Courier_List> builder)
        {
            builder
                .ToTable("Courier_List");
            builder
                .HasKey(w => w.Id);
            builder
              .Property(t => t.Courier_Name)
              .HasColumnName("Courier_Name");
            builder
                .Property(t => t.Contact_Person)
                .HasColumnName("Contact_Person");
            builder
                .Property(t => t.Contact_Phone)
                .HasColumnName("Contact_Phone");
           

            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Courier_List_TenantId");
        }
    }
}
