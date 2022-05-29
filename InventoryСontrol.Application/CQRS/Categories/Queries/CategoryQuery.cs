using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using InventoryСontrol.Application.CQRS.Categories.Views;
using InventoryСontrol.Storage.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InventoryСontrol.Application.CQRS.Categories.Queries
{
    public sealed class CategoryQuery : ICategoryQuery
    {
        private static InventoryСontrolContext _context;
        private static IMapper _mapper;

        public CategoryQuery(
            InventoryСontrolContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryView>> GetAllCategoriesAsync()
        {
            var result = await _context.Categories.ToArrayAsync();

            return _mapper.Map<IEnumerable<CategoryView>>(result);
        }
    }
}