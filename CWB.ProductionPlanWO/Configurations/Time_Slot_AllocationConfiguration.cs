using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CWB.ProductionPlanWO.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CWB.ProductionPlanWO.Configurations
{
    public class Time_Slot_AllocationConfiguration : IEntityTypeConfiguration<Time_Slot_Allocation>
    {
        public void Configure(EntityTypeBuilder<Time_Slot_Allocation> builder)
        {
            builder
                .ToTable("Time_Slot_Allocation");
            builder
                .HasKey(c => c.Id);
            builder
                .Property(c => c.Allocation_Desc)
                .HasColumnName("Allocation_Desc")
                .IsRequired();
            //builder.ConfigureBase();

            builder.HasData(
        new Time_Slot_Allocation { Id = 1, Allocation_Desc = "Not Allocated" },
        new Time_Slot_Allocation { Id = 2, Allocation_Desc = "Test" },
        new Time_Slot_Allocation { Id = 3, Allocation_Desc = "Allocated" },
        new Time_Slot_Allocation { Id = 4, Allocation_Desc = "Complete" },
        new Time_Slot_Allocation { Id = 5, Allocation_Desc = "In Machine" }
    );


        }
    }
}
