using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CWB.ProductionPlanWO.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CWB.ProductionPlanWO.Configurations
{
    public class Mode_ListConfiguration : IEntityTypeConfiguration<Mode_List>
    {
        public void Configure(EntityTypeBuilder<Mode_List> builder)
        {
            builder
                .ToTable("Mode_List");
            builder
                .HasKey(c => c.Id);
            builder
                .Property(c => c.Mode_Desc)
                .HasColumnName("Mode_Desc")
                .IsRequired();
            //builder.ConfigureBase();

            builder.HasData(
        new Mode_List { Id = 1, Mode_Desc = "Test" },
        new Mode_List { Id = 2, Mode_Desc = "Freeze" }
    );


        }
    }
}
