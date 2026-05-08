using CWB.CompanySettings.Domain;
using CWB.CommonUtils.Common.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.CompanySettings.Configurations
{
    public class Stores_Dept_IDListConfiguration : IEntityTypeConfiguration<Stores_Dept_IDList>
    {
        public void Configure(EntityTypeBuilder<Stores_Dept_IDList> builder)
        {
            builder
                .ToTable("Stores_Dept_IDList");
            builder
                .HasKey(c => c.Id);
            builder
                .Property(c => c.Stores_DirMatl_ID)
                .HasColumnName("Stores_DirMatl_ID");
            builder
                .Property(c => c.Stores_Cust_Dispatch_ID)
                .HasColumnName("Stores_Cust_Dispatch_ID");
            builder
                .Property(c => c.Stores_Tools_ID)
                .HasColumnName("Stores_Tools_ID");
            builder
                .Property(c => c.Stores_Consumables_ID)
                .HasColumnName("Stores_Consumables_ID");


            //builder.ConfigureBase();





        }
    }
}
