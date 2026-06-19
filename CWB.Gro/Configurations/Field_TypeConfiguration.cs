using CWB.Gro.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CWB.Gro.Configurations
{
    public class Field_TypeConfiguration : IEntityTypeConfiguration<Field_Type>
    {
        public void Configure(EntityTypeBuilder<Field_Type> builder)
        {
            builder
                .ToTable("Field_Type");
            builder
                .HasKey(c => c.Id);
            builder
                .Property(c => c.Field_Type_Desc)
                .HasColumnName("Field_Type_Desc")
                .IsRequired();
            //builder.ConfigureBase();

            builder.HasData(
        new Field_Type { Id = 1, Field_Type_Desc = "Integer" },
        new Field_Type { Id = 2, Field_Type_Desc = "string" },
        new Field_Type { Id = 3, Field_Type_Desc = "Date" }
    );


        }
    }
}
