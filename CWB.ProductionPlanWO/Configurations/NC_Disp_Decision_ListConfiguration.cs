using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CWB.CommonUtils.Common.Configurations;
using CWB.ProductionPlanWO.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CWB.ProductionPlanWO.Configurations
{
    public class NC_Disp_Decision_ListConfiguration : IEntityTypeConfiguration<NC_Disp_Decision_List>
    {
        public void Configure(EntityTypeBuilder<NC_Disp_Decision_List> builder)
        {
            builder
                .ToTable("NC_Disp_Decision_List");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.NC_Disp_Decision_Desc)
                .HasColumnName("NC_Disp_Decision_Desc");
            //builder.ConfigureBase();
            builder.HasData(
        new NC_Disp_Decision_List { Id = 1, NC_Disp_Decision_Desc = "Accepted / Bookout" },
        new NC_Disp_Decision_List { Id = 2, NC_Disp_Decision_Desc = "Reinspect & Bookout" },
        new NC_Disp_Decision_List { Id = 3, NC_Disp_Decision_Desc = "Concessionally Accepted &Bookout" },
        new NC_Disp_Decision_List { Id = 4, NC_Disp_Decision_Desc = "Inhouse Rework" },
        new NC_Disp_Decision_List { Id = 5, NC_Disp_Decision_Desc = "Send to Supplier for Rework" },
        new NC_Disp_Decision_List { Id = 6, NC_Disp_Decision_Desc = "Rework on behalf of Supplier" },
        new NC_Disp_Decision_List { Id = 7, NC_Disp_Decision_Desc = "Scrap & dispose" },
        new NC_Disp_Decision_List { Id = 8, NC_Disp_Decision_Desc = "Scrap & Convert to new part" },
        new NC_Disp_Decision_List { Id = 9, NC_Disp_Decision_Desc = "Scrap & Salvage Child Parts" }
    );
        }
    }
}
