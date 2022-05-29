using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InventoryСontrol.Api.Persistence;
using InventoryСontrol.Application.CQRS.Items.Commands;
using InventoryСontrol.Application.CQRS.Items.Queries;
using InventoryСontrol.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InventoryСontrol.Api.Controllers
{
    [Route("api/items")]
    public class ItemController : Controller
    {
        private readonly IItemCommand _iItemCommand;
        private readonly IItemQuery _iItemQuery;
        private readonly UserManager<User> _userManager;

        public ItemController(
            IItemQuery iItemQuery,
            IItemCommand iItemCommand,
            UserManager<User> userManager)
        {
            _iItemQuery = iItemQuery;
            _iItemCommand = iItemCommand;
            _userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllItems()
        {
            var result = await _iItemQuery.GetAllItemsAsync();
            return Ok(result);
        }


        [HttpGet("{name}")]
        [AllowAnonymous]
        public async Task<IActionResult> Search([FromRoute] string name)
        {
            var result = await _iItemQuery.SearchAsync(name);
            return Ok(result);
        }

        [HttpPut("update")]
        [Authorize(Policy = Policies.AdminOrManager)]
        public async Task<IActionResult> UpdateItem(
            [FromQuery] Guid itemId,
            [FromQuery] string name,
            [FromQuery] int? amount,
            [FromQuery] int? cost)
        {
            var result = await _iItemCommand.UpdateAsync(itemId, name, amount, cost);
            return Ok(result);
        }

        [HttpGet("filter")]
        [AllowAnonymous]
        public async Task<IActionResult> GetItemsByFilterAndSorting(
            [FromQuery] string name,
            [FromQuery] int? costFrom,
            [FromQuery] int? costTo,
            [FromQuery] List<string> categories,
            [FromQuery] bool? sortIsAscending = true)
        {
            var result = await _iItemQuery.GetItemsByFilterAndSortingAsync(
                name,
                costFrom,
                costTo,
                categories,
                sortIsAscending);
            return Ok(result);
        }

        [HttpPost("add")]
        [Authorize(Policy = Policies.AdminOrManager)]
        public async Task<IActionResult> AddItem(
            [FromQuery] string name,
            [FromQuery] int amount,
            [FromQuery] int cost)
        {
            var result = await _iItemCommand.AddAsync(
                name,
                amount,
                cost);
            return Ok(result);
        }

        [HttpPut("buy")]
        [Authorize(Policy = Policies.User)]
        public async Task<IActionResult> BuyItem(
            [FromQuery] Guid itemId,
            [FromQuery] int amount)
        {
            await _iItemCommand.BuyAsync(
                itemId,
                amount);
            return Ok();
        }

        [HttpPost("preorder")]
        [Authorize(Policy = Policies.User)]
        public async Task<IActionResult> PreOrder(
            [FromQuery] Guid itemId,
            [FromQuery] int amount)
        {
            var userId = (await _userManager.FindByNameAsync(User.Identity.Name)).Id;

            await _iItemCommand.PreOrderAsync(
                userId,
                itemId,
                amount);
            return Ok();
        }

        [HttpPut("{itemId}/categories/{categoryId}/add")]
        [Authorize(Policy = Policies.AdminOrManager)]
        public async Task<IActionResult> AddCategoryToItem(
            [FromRoute] Guid itemId,
            [FromRoute] Guid categoryId)
        {
            var result = await _iItemCommand.AddCategoryToItemAsync(
                itemId,
                categoryId);
            return Ok(result);
        }
    }
}