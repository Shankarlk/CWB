using System;
using System.Collections.Generic;
using System.Linq;
using CWB.CommonUtils.Common.Configurations;
using CWB.CompanySettings.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Threading.Tasks;

namespace CWB.CompanySettings.Configurations
{
    public class TimeSlotDurationConfiguration : IEntityTypeConfiguration<TimeSlotDuration>
    {
        public void Configure(EntityTypeBuilder<TimeSlotDuration> builder)
        {

            builder
                  .ToTable("TimeSlotDuration");
            builder.ConfigureBase();
            builder
                .Property(w => w.Duration)
                .HasColumnName("Duration");
            builder.HasData(
            new TimeSlotDuration { Id = 1, Duration = 5 },
            new TimeSlotDuration { Id = 2, Duration = 10 },
            new TimeSlotDuration { Id = 3, Duration = 15 },
            new TimeSlotDuration { Id = 4, Duration = 30 }
            );

        }
    }
}
