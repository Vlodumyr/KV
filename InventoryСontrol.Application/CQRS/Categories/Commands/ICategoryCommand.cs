using System;
using System.Threading.Tasks;
using InventoryСontrol.Application.CQRS.Categories.Views;

namespace InventoryСontrol.Application.CQRS.Categories.Commands
{
    public interface ICategoryCommand
    {
        public Task<CategoryView> AddAsync(
            string name);

        public Task<CategoryView> UpdateAsync(
            Guid categoryId,
            string name);
    }
}