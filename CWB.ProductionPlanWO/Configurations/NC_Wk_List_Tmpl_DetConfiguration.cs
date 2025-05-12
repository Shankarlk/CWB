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
    public class NC_Wk_List_Tmpl_DetConfiguration : IEntityTypeConfiguration<NC_Wk_List_Tmpl_Det>
    {
        public void Configure(EntityTypeBuilder<NC_Wk_List_Tmpl_Det> builder)
        {
            builder
                .ToTable("NC_Wk_List_Tmpl_Det");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.NC_Wk_List_Tmpl_Appl_Id)
                .HasColumnName("NC_Wk_List_Tmpl_Appl_Id");
            builder
                .Property(b => b.NC_Wk_Step_Desc)
                .HasColumnName("NC_Wk_Step_Desc");
            builder
                .Property(b => b.Resp_Dept)
                .HasColumnName("Resp_Dept");
            builder
                .Property(b => b.UI_ID)
                .HasColumnName("UI_ID");
            builder
                .Property(b => b.Seq_no)
                .HasColumnName("Seq_no");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("NC_Wk_List_Tmpl_Det_TenantId");
        }
    }
}
