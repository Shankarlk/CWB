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
    public class Cont_RCA_CA_Status_ListConfiguration : IEntityTypeConfiguration<Cont_RCA_CA_Status_List>
    {
        public void Configure(EntityTypeBuilder<Cont_RCA_CA_Status_List> builder)
        {
            builder
                .ToTable("Cont_RCA_CA_Status_List");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.Status_Desc)
                .HasColumnName("Status_Desc");
            //builder.ConfigureBase();
            builder.HasData(
        new Cont_RCA_CA_Status_List { Id = 1, Status_Desc = "Not Uploaded" },
        new Cont_RCA_CA_Status_List { Id = 2, Status_Desc = "Uploaded" },
        new Cont_RCA_CA_Status_List { Id = 3, Status_Desc = "Uploaded" },
        new Cont_RCA_CA_Status_List { Id = 4, Status_Desc = "Correct / Resubmit" }
    );
        }
    }
}
