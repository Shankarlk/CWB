using CWB.CommonUtils.Common.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CWB.Masters.Configurations
{
    public class RoutingStepMachineConfiguration : IEntityTypeConfiguration<Domain.RoutingStepMachine>
    {
        /*
         * `Id` bigint NOT NULL AUTO_INCREMENT,
  `TenantId` bigint NOT NULL,
  `MachineId` bigint NOT NULL,
  `RoutingStepId` bigint NOT NULL,
  `SetupTime` time(6) NOT NULL,
  `FloorToFloorTime` time(6) NOT NULL,
  `FirstPieceProcessingTime` time(6) NOT NULL,
  `NoOfPartsPerLoading` int NOT NULL,
         */

        public void Configure(EntityTypeBuilder<Domain.RoutingStepMachine> builder)
        {
            builder
             .ToTable("RoutingStepMachine");
            builder
               .HasKey(m => m.Id);
            builder
                .Property(m => m.RoutingStepId)
                .HasColumnName("RoutingStepId")
                .IsRequired();
            builder
               .Property(m => m.MachineId)
               .HasColumnName("MachineId")
               .IsRequired();
            builder
               .Property(m => m.SetupTime)
               .HasColumnName("SetupTime")
               .IsRequired();
            builder
              .Property(m => m.FloorToFloorTime)
              .HasColumnName("FloorToFloorTime")
              .HasMaxLength(255)
              .IsRequired();
            builder
            .Property(m => m.FirstPieceProcessingTime)
            .HasColumnName("FirstPieceProcessingTime")
            .HasMaxLength(255)
            .IsRequired();
            builder
                .Property(m => m.NoOfPartsPerLoading)
          .HasColumnName("NoOfPartsPerLoading")
          .HasMaxLength(255)
          .IsRequired();
            builder
                .Property(m => m.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
        }
    }
}
