using System;
using System.Collections.Generic;

namespace InventoryСontrol.Application.CQRS.Items.Views
{
    public class ItemView
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
        public int Amount { get; set; }
        public List<string> Categories { get; set; }
    }
}