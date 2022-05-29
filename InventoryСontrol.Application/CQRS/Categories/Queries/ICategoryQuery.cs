using System.Collections.Generic;
using System.Threading.Tasks;
using InventoryСontrol.Application.CQRS.Categories.Views;

namespace InventoryСontrol.Application.CQRS.Categories.Queries
{
    public interface ICategoryQuery
    {
        public Task<IEnumerable<CategoryView>> GetAllCategoriesAsync();
    }
}