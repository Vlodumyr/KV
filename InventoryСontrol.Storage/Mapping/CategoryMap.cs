using InventoryСontrol.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryСontrol.Storage.Mapping
{
    public sealed class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Category");
            builder.HasKey(item => item.Name);

            builder
                .HasIndex(item => item.Name)
                .IsUnique();
        }
    }
}