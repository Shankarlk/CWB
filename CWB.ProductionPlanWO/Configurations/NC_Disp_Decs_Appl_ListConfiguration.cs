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
    public class NC_Disp_Decs_Appl_ListConfiguration : IEntityTypeConfiguration<NC_Disp_Decs_Appl_List>
    {
        public void Configure(EntityTypeBuilder<NC_Disp_Decs_Appl_List> builder)
        {
            builder
                .ToTable("NC_Disp_Decs_Appl_List");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.NC_Disp_Decision_Id)
                .HasColumnName("NC_Disp_Decision_Id");
            builder
                .Property(b => b.Level_2_Dec_Reqd)
                .HasColumnName("Level_2_Dec_Reqd");
            builder
                .Property(b => b.Rm)
                .HasColumnName("Rm");
            builder
                .Property(b => b.Bof)
                .HasColumnName("Bof");
            builder
                .Property(b => b.SubCon)
                .HasColumnName("SubCon");
            builder
                .Property(b => b.Cmp)
                .HasColumnName("Cmp");
            builder
                .Property(b => b.Assy)
                .HasColumnName("Assy");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("NC_Disp_Decs_Appl_List_TenantId");
        }
    }
}
