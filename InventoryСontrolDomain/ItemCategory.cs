using System;

namespace InventoryСontrol.Domain
{
    public class ItemCategory
    {
        public Guid ItemCategoryId { get; set; }
        public Item Item { get; set; }
        public Category Category { get; set; }
        public Guid ItemId { get; set; }
        public Guid CategoryId { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? UpdatedOnUtc { get; set; }

        public static ItemCategory Create(
            Item item,
            Category category)
        {
            return new ItemCategory
            {
                Item = item,
                Category = category,
                ItemId = item.ItemId,
                CategoryId = category.CategoryId,
                CreatedOnUtc = DateTime.UtcNow
            };
        }
    }
}