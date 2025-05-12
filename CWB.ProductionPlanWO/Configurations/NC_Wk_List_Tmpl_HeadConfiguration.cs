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
    public class NC_Wk_List_Tmpl_HeadConfiguration : IEntityTypeConfiguration<NC_Wk_List_Tmpl_Head>
    {
        public void Configure(EntityTypeBuilder<NC_Wk_List_Tmpl_Head> builder)
        {
            builder
                .ToTable("NC_Wk_List_Tmpl_Head");
            builder
                .HasKey(b => b.Id);
            builder
                .Property(b => b.NC_Disp_Decision_Id)
                .HasColumnName("NC_Disp_Decision_Id");
            builder
                .Property(b => b.No_of_steps)
                .HasColumnName("No_of_steps");
            builder
                .Property(c => c.TenantId)
                .HasColumnName("TenantId")
                .IsRequired();
            builder.ConfigureBase();
            builder.HasIndex(c => c.TenantId).HasDatabaseName("NC_Wk_List_Tmpl_Head_TenantId");
        }
    }
}
