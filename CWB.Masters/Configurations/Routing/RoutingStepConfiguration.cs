using CWB.CommonUtils.Common.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CWB.Masters.Configurations
{
    public class RoutingStepConfiguration
        : IEntityTypeConfiguration<Domain.RoutingStep>
    {
        /***
                 *  `Id` bigint NOT NULL AUTO_INCREMENT,
          `RoutingStepId` bigint NOT NULL,
          `ManufacturedPartId` bigint NOT NULL,/*Refers to Id from ManufactredPartNoDetails
          `BOMId` bigint not NULL, /*Refers to BOM entry from MPBOMs table
          `QuantityAssembly` int NOT NULL,
         */
        public void Configure(EntityTypeBuilder<Domain.RoutingStep> builder)
        {
            builder
             .ToTable("RoutingStep");
            builder
               .HasKey(m => m.Id);
            builder
                .Property(m => m.RoutingId)
                .HasColumnName("RoutingId")
                .IsRequired();
            builder
              .Property(m => m.StepNumber)
              .HasColumnName("StepNumber")
              .HasMaxLength(255)
              .IsRequired();
            builder
             .Property(m => m.StepDescription)
             .HasColumnName("StepDescription")
             .HasMaxLength(300);

            builder
               .Property(t => t.RoutingStepOperation)
               .HasColumnName("RoutingStepOperation")
               .IsRequired();
            builder
               .Property(t => t.RoutingStepLocation)
               .HasColumnName("RoutingStepLocation")
               .IsRequired();
            builder
             .Property(t => t.RoutingStepSequence)
             .HasColumnName("RoutingStepSequence")
             .IsRequired();
            builder
                .Property(m => m.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
        }
    }
}
