using CWB.ProductionPlanWO.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Configurations
{
    public class Inv_Trans_ListConfiguration : IEntityTypeConfiguration<Inv_Trans_List>
    {
        public void Configure(EntityTypeBuilder<Inv_Trans_List> builder)
        {
            builder
                .ToTable("Inv_Trans_List");
            builder
                .HasKey(c => c.Id);
            builder
                .Property(c => c.Inv_Trans_Desc)
                .HasColumnName("Inv_Trans_Desc")
                .IsRequired();
            //builder.ConfigureBase();

            builder.HasData(
        new Inv_Trans_List { Id = 1, Inv_Trans_Desc = "Inward RM / BOF" },
        new Inv_Trans_List { Id = 2, Inv_Trans_Desc = "Inward SubCon (Fin Part)" },
        new Inv_Trans_List { Id = 3, Inv_Trans_Desc = "Return Unprocessed Parts to Stores from Subcon" },
        new Inv_Trans_List { Id = 4, Inv_Trans_Desc = "Return Unprocessed Parts to Stores from Shop" },
        new Inv_Trans_List { Id = 5, Inv_Trans_Desc = "Issue Shop" },
        new Inv_Trans_List { Id = 6, Inv_Trans_Desc = "Issue SubCon" },
        new Inv_Trans_List { Id = 7, Inv_Trans_Desc = "Within Shop Bookout" },
        new Inv_Trans_List { Id = 8, Inv_Trans_Desc = "Bookout from Shop" },
        new Inv_Trans_List { Id = 9, Inv_Trans_Desc = "Dispatch" },
        new Inv_Trans_List { Id = 10, Inv_Trans_Desc = "Move to Scrap" }
    );


        }
    }
}
