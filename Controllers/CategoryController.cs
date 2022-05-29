using System;
using System.Threading.Tasks;
using InventoryСontrol.Api.Persistence;
using InventoryСontrol.Application.CQRS.Categories.Commands;
using InventoryСontrol.Application.CQRS.Categories.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryСontrol.Api.Controllers
{
    [Route("api/categories")]
    public class CategoryController : Controller
    {
        private readonly ICategoryCommand _iCategoryCommand;
        private readonly ICategoryQuery _iCategoryQuery;

        public CategoryController(
            ICategoryQuery iCategoryQuery,
            ICategoryCommand iCategoryCommand)
        {
            _iCategoryCommand = iCategoryCommand;
            _iCategoryQuery = iCategoryQuery;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCategories()
        {
            var result = await _iCategoryQuery.GetAllCategoriesAsync();
            return Ok(result);
        }

        [HttpPost("add")]
        [Authorize(Policy = Policies.AdminOrManager)]
        public async Task<IActionResult> AddCategory(
            [FromQuery] string name)
        {
            var result = await _iCategoryCommand.AddAsync(name);
            return Ok(result);
        }

        [HttpPut("update")]
        [Authorize(Policy = Policies.AdminOrManager)]
        public async Task<IActionResult> UpdateCategory(
            [FromQuery] Guid categoryId,
            [FromQuery] string name)
        {
            var result = await _iCategoryCommand.UpdateAsync(categoryId, name);
            return Ok(result);
        }
    }
}