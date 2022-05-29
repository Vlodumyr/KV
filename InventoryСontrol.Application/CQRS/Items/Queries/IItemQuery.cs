using System.Collections.Generic;
using System.Threading.Tasks;
using InventoryСontrol.Application.CQRS.Items.Views;

namespace InventoryСontrol.Application.CQRS.Items.Queries
{
    public interface IItemQuery
    {
        public Task<IEnumerable<ItemView>> GetAllItemsAsync();

        public Task<IEnumerable<ItemView>> SearchAsync(string name);

        public Task<IEnumerable<ItemView>> GetItemsByFilterAndSortingAsync(
            string name,
            int? costFrom,
            int? costTo,
            List<string> categories,
            bool? sortIsAscending = true);
    }
}