using CWB.CommonUtils.Common.Configurations;
using CWB.ProductionPlanWO.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.ProductionPlanWO.Configurations
{
    public class Inv_Mismatch_ListConfiguration : IEntityTypeConfiguration<Inv_Mismatch_List>
    {
        public void Configure(EntityTypeBuilder<Inv_Mismatch_List> builder)
        {
            builder
                .ToTable("Inv_Mismatch_List");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.Calling_UI_ID)
                .HasColumnName("Calling_UI_ID");
            builder
                .Property(b => b.Reported_By)
                .HasColumnName("Reported_By");
            builder
                .Property(b => b.Report_date)
                .HasColumnName("Report_date");
            builder
                .Property(b => b.Record_No)
                .HasColumnName("Record_No");
            builder
                .Property(b => b.Part_No)
                .HasColumnName("Part_No");
            builder
                .Property(b => b.Opr_No)
                .HasColumnName("Opr_No");
            builder
                .Property(b => b.Routing_ID)
                .HasColumnName("Routing_ID");
            builder
                .Property(b => b.Location_ID)
                .HasColumnName("Location_ID");
            builder
                .Property(b => b.PO_Ref)
                .HasColumnName("PO_Ref");
            builder
                .Property(b => b.Mismatch_Qnty)
                .HasColumnName("Mismatch_Qnty");
            builder
                .Property(b => b.Mismatch_Comments)
                .HasColumnName("Mismatch_Comments");
            builder
                .Property(b => b.Mismatch_Status)
                .HasColumnName("Mismatch_Status");
            builder
                .Property(b => b.Resolved)
                .HasColumnName("Resolved");
            builder
                .Property(b => b.Resolved_by)
                .HasColumnName("Resolved_by");
            builder
                .Property(b => b.Resolution_date)
                .HasColumnName("Resolution_date");
            builder
                .Property(b => b.Resolution_Comments)
                .HasColumnName("Resolution_Comments");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Inv_Mismatch_List_TenantId");
        }
    }
}
