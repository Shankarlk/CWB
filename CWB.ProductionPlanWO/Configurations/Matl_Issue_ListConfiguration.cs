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
    public class Matl_Issue_ListConfiguration : IEntityTypeConfiguration<Matl_Issue_List>
    {
        public void Configure(EntityTypeBuilder<Matl_Issue_List> builder)
        {
            builder
                .ToTable("Matl_Issue_List");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.Part_Ref)
                .HasColumnName("Part_Ref");
            builder
                .Property(b => b.Issue_Qnty)
                .HasColumnName("Issue_Qnty");
            builder
                .Property(b => b.Issue_Mov_date)
                .HasColumnName("Issue_Mov_date");
            builder
                .Property(b => b.Mode)
                .HasColumnName("Mode");
            builder
                .Property(b => b.Immediate_Movmt)
                .HasColumnName("Immediate_Movmt");
            builder
                .Property(b => b.Issue_Mov_Compl)
                .HasColumnName("Issue_Mov_Compl");
            builder
                .Property(b => b.From_Location)
                .HasColumnName("From_Location");
            builder
                .Property(b => b.To_Location)
                .HasColumnName("To_Location");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("Matl_Issue_List_TenantId");
        }
    }
}
