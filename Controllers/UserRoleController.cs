using System;
using System.Threading.Tasks;
using InventoryСontrol.Api.Persistence;
using InventoryСontrol.Application.CQRS.UserAccounts.Role.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryСontrol.Api.Controllers
{
    [Route("api/users")]
    public class UserRoleController : Controller
    {
        private static IUserRoleCommand _iUserRoleCommand;

        public UserRoleController(IUserRoleCommand iUserRoleCommand)
        {
            _iUserRoleCommand = iUserRoleCommand;
        }

        [HttpPut("{userId}/role/{role}/attach")]
        [Authorize(Policy = Policies.Admin)]
        public async Task<IActionResult> AttachRole(
            [FromRoute] Guid userId,
            [FromRoute] Roles role)
        {
            var result = await _iUserRoleCommand.AddUserRoleAsync(userId, role.ToString());
            return Ok(result);
        }

        [HttpPut("{userId}/role/{role}/remove")]
        [Authorize(Policy = Policies.Admin)]
        public async Task<IActionResult> RemoveRole(
            [FromRoute] Guid userId,
            [FromRoute] Roles role)
        {
            var result = await _iUserRoleCommand.RemoveUserRoleAsync(userId, role.ToString());
            return Ok(result);
        }
    }
}