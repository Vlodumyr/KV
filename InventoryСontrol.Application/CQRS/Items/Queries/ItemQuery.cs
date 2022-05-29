using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InventoryСontrol.Application.CQRS.Items.Views;
using InventoryСontrol.Storage.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventoryСontrol.Application.CQRS.Items.Queries
{
    public sealed class ItemQuery : IItemQuery
    {
        private static InventoryСontrolContext _context;
        private static IMapper _mapper;

        public ItemQuery(
            InventoryСontrolContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ItemView>> GetAllItemsAsync()
        {
            var result = await _context.Items
                .Include(i => i.Categories)
                .ThenInclude(i => i.Category)
                .ToArrayAsync();

            return _mapper.Map<IEnumerable<ItemView>>(result);
        }

        public async Task<IEnumerable<ItemView>> SearchAsync(string name)
        {
            var result = await _context.Items
                .Include(i => i.Categories)
                .ThenInclude(i => i.Category)
                .Where(i => i.Name.Contains(name))
                .ToArrayAsync();

            return _mapper.Map<IEnumerable<ItemView>>(result);
        }

        public async Task<IEnumerable<ItemView>> GetItemsByFilterAndSortingAsync(
            string name,
            int? costFrom,
            int? costTo,
            List<string> categories,
            bool? sortIsAscending = true)
        {
            var result = await _context.Items
                .Include(i => i.Categories)
                .ThenInclude(i => i.Category)
                .ToArrayAsync();

            if (categories != null && categories.Any() && categories.Exists(i => i != null))
                result = result
                    .Where(i => i.Categories.Any(x => categories.Contains(x.Category.Name)))
                    .ToArray();

            if (!string.IsNullOrWhiteSpace(name)) result = result.Where(i => i.Name.Contains(name)).ToArray();

            if (costFrom.HasValue) result = result.Where(i => i.Cost >= costFrom).ToArray();

            if (costTo.HasValue) result = result.Where(i => i.Cost <= costTo).ToArray();

            if (sortIsAscending.HasValue)
            {
                if (sortIsAscending.Value)
                    result.OrderBy(i => i.UpdatedOnUtc);

                else
                    result.OrderByDescending(i => i.UpdatedOnUtc);
            }


            return _mapper.Map<IEnumerable<ItemView>>(result);
        }
    }
}