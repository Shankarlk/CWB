using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CWB.CommonUtils.Common.Configurations;
using CWB.CompanySettings.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CWB.CompanySettings.Configurations
{
    public class NoOfDaysTimeSlotConfiguration : IEntityTypeConfiguration<NoOfDaysTimeSlot>
    {
        public void Configure(EntityTypeBuilder<NoOfDaysTimeSlot> builder)
        {

            builder
                  .ToTable("NoOfDaysTimeSlot");
            builder.ConfigureBase();
            builder
                .Property(w => w.Days)
                .HasColumnName("Duration");
            builder.HasData(
            new NoOfDaysTimeSlot { Id = 1, Days = 30},
            new NoOfDaysTimeSlot { Id = 2, Days =  60},
            new NoOfDaysTimeSlot { Id = 3, Days = 90 },
            new NoOfDaysTimeSlot { Id = 4, Days =  120},
            new NoOfDaysTimeSlot { Id = 5, Days =  150}
            );

        }
    }
}
