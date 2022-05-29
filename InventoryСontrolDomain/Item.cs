using System;
using System.Collections.Generic;

namespace InventoryСontrol.Domain
{
    public class Item
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
        public int Amount { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? UpdatedOnUtc { get; set; }
        public List<ItemCategory> Categories { get; set; }
        public List<PreOrder> PreOrders { get; set; }

        public static Item Create(
            string name,
            int cost,
            int amount)
        {
            return new Item
            {
                Name = name,
                Cost = cost,
                Amount = amount,
                CreatedOnUtc = DateTime.UtcNow
            };
        }

        public Item UpdateAmount(int amount)
        {
            Amount = amount;
            UpdatedOnUtc = DateTime.UtcNow;
            return this;
        }

        public Item UpdateCost(int cost)
        {
            Cost = cost;
            UpdatedOnUtc = DateTime.UtcNow;
            return this;
        }

        public Item UpdateName(string name)
        {
            Name = name;
            UpdatedOnUtc = DateTime.UtcNow;
            return this;
        }
    }
}