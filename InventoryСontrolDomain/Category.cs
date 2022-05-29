using System;
using System.Collections.Generic;

namespace InventoryСontrol.Domain
{
    public class Category
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? UpdatedOnUtc { get; set; }
        public List<ItemCategory> Items { get; set; }

        public static Category Create(string name)
        {
            return new Category
            {
                Name = name,
                CreatedOnUtc = DateTime.UtcNow
            };
        }

        public Category Update(string name)
        {
            Name = name;
            UpdatedOnUtc = DateTime.UtcNow;
            return this;
        }
    }
}