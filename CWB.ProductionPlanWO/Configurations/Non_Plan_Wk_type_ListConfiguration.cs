using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CWB.ProductionPlanWO.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CWB.ProductionPlanWO.Configurations
{
    public class Non_Plan_Wk_type_ListConfiguration : IEntityTypeConfiguration<Non_Plan_Wk_type_List>
    {
        public void Configure(EntityTypeBuilder<Non_Plan_Wk_type_List> builder)
        {
            builder
                .ToTable("Non_Plan_Wk_type_List");
            builder
                .HasKey(c => c.Id);
            builder
                .Property(c => c.Non_Plan_Wk_type_Desc)
                .HasColumnName("Non_Plan_Wk_type_Desc")
                .IsRequired();
            //builder.ConfigureBase();

            builder.HasData(
        new Non_Plan_Wk_type_List { Id = 1, Non_Plan_Wk_type_Desc = "Prototype" },
        new Non_Plan_Wk_type_List { Id = 2, Non_Plan_Wk_type_Desc = "Fixture Part Manf" },
        new Non_Plan_Wk_type_List { Id = 3, Non_Plan_Wk_type_Desc = "Fixture Rework" },
        new Non_Plan_Wk_type_List { Id = 4, Non_Plan_Wk_type_Desc = "Tool Manf" },
        new Non_Plan_Wk_type_List { Id = 5, Non_Plan_Wk_type_Desc = "Tool Rework" }
    );


        }
    }
}
