using InventoryСontrol.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryСontrol.Storage.Mapping
{
    public sealed class ItemCategoryMap : IEntityTypeConfiguration<ItemCategory>
    {
        public void Configure(EntityTypeBuilder<ItemCategory> builder)
        {
            builder.ToTable("ItemCategory");
            builder.HasKey(item => item.ItemCategoryId);

            builder
                .HasOne(item => item.Item)
                .WithMany(item => item.Categories)
                .HasForeignKey(item => item.ItemId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(item => item.Category)
                .WithMany(item => item.Items)
                .HasForeignKey(item => item.CategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}