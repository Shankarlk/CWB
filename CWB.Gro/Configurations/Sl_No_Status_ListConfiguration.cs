using CWB.Gro.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CWB.Gro.Configurations
{
    public class Sl_No_Status_ListConfiguration : IEntityTypeConfiguration<Sl_No_Status_List>
    {
        public void Configure(EntityTypeBuilder<Sl_No_Status_List> builder)
        {
            builder
                .ToTable("Sl_No_Status_List");
            builder
                .HasKey(c => c.Id);
            builder
                .Property(c => c.Sl_No_Status_Desc)
                .HasColumnName("Sl_No_Status_Desc")
                .IsRequired();
            //builder.ConfigureBase();

            builder.HasData(
        new Sl_No_Status_List { Id = 1, Sl_No_Status_Desc = "Printed" },
        new Sl_No_Status_List { Id = 2, Sl_No_Status_Desc = "1st Scan" },
        new Sl_No_Status_List { Id = 3, Sl_No_Status_Desc = "Assigned" },
        new Sl_No_Status_List { Id = 4, Sl_No_Status_Desc = "Sent" },
        new Sl_No_Status_List { Id = 5, Sl_No_Status_Desc = "Deleted" }
    );


        }
    }
}
