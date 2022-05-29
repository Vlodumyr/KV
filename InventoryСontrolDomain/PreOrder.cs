using System;
using InventoryСontrol.Domain.Enum;

namespace InventoryСontrol.Domain
{
    public class PreOrder
    {
        public Guid PreOrderId { get; set; }
        public Item Item { get; set; }
        public User User { get; set; }
        public Guid ItemId { get; set; }
        public Guid UserId { get; set; }
        public int Amount { get; set; }
        public PreOrderStatus Status { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? UpdatedOnUtc { get; set; }

        public static PreOrder Create(
            Item item,
            User user,
            int amount)
        {
            return new PreOrder
            {
                Item = item,
                User = user,
                ItemId = item.ItemId,
                UserId = Guid.Parse(user.Id),
                Amount = amount,
                Status = PreOrderStatus.Pending,
                CreatedOnUtc = DateTime.UtcNow
            };
        }

        public PreOrder Update(PreOrderStatus status)
        {
            Status = status;
            UpdatedOnUtc = DateTime.UtcNow;
            return this;
        }

        public bool CheckAvailableItem()
        {
            return Item.Amount < Amount;
        }
    }
}