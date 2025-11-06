using CWB.ProductionPlanWO.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Configurations
{
    public class Cust_NC_DecisionConfiguration : IEntityTypeConfiguration<Cust_NC_Decision>
    {
        public void Configure(EntityTypeBuilder<Cust_NC_Decision> builder)
        {
            builder
                .ToTable("Cust_NC_Decision");
            builder
                .HasKey(c => c.Id);
            builder
                .Property(c => c.Cust_Decision)
                .HasColumnName("Cust_Decision")
                .IsRequired();
            //builder.ConfigureBase();

            builder.HasData(
        new Cust_NC_Decision { Id = 1, Cust_Decision = "Scrap" },
        new Cust_NC_Decision { Id = 2, Cust_Decision = "Reworked by them" },
        new Cust_NC_Decision { Id = 3, Cust_Decision = "Concessionally Accepted" },
        new Cust_NC_Decision { Id = 4, Cust_Decision = "Send back for Rework" },
        new Cust_NC_Decision { Id = 5, Cust_Decision = "Others" }
    );


        }
    }
}
