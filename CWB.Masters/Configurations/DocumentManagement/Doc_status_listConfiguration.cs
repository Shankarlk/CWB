using CWB.CommonUtils.Common.Configurations;
using CWB.Masters.Domain.DocumentManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CWB.Masters.Configurations.DocumentManagement
{
    public class Doc_status_listConfiguration : IEntityTypeConfiguration<Doc_status_list>
    {
        public void Configure(EntityTypeBuilder<Doc_status_list> builder)
        {
            builder
            .ToTable("Doc_status_list");
            builder
               .HasKey(c => c.Id);
            builder
                .Property(t => t.Doc_Status_Desc)
                .HasColumnName("Doc_Status_Desc")
                .IsRequired();
            builder.ConfigureBase();

            builder.HasData(
        new Doc_status_list { Id = 1, Doc_Status_Desc = "w/f  Approval" },
        new Doc_status_list { Id = 2, Doc_Status_Desc = "Approved" },
        new Doc_status_list { Id = 3, Doc_Status_Desc = "Archive" }
        //new Doc_status_list { Id = 4, Doc_Status_Desc = "On Hold" }
    );
        }
    }
}
