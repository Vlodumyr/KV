using System.Collections.Generic;
using System.Threading.Tasks;
using InventoryСontrol.Api.Extensions.Validator;
using InventoryСontrol.Api.Persistence;
using InventoryСontrol.Application.CQRS.UserAccounts.Users.Queries;
using InventoryСontrol.Application.CQRS.UserAccounts.Views;
using InventoryСontrol.Application.Extensions;
using InventoryСontrol.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InventoryСontrol.Api.Controllers
{
    [Route("api/users")]
    public class AccountController : ControllerBase
    {
        private readonly IUserAccountQuery _iUserAccountQuery;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IUserAccountQuery iUserAccountQuery)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _iUserAccountQuery = iUserAccountQuery;
        }

        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp(
            [FromQuery] string email,
            [FromQuery] string password)
        {
            if (!EmailValidator.IsValid(email)) return BadRequest();

            var a = await new PasswordValidator(_userManager).IsValidAsync(password);

            if (!await new PasswordValidator(_userManager).IsValidAsync(password)) return BadRequest();

            var user = Domain.User.Create(email);

            var result = await _userManager.CreateAsync(user, password);

            if (result.TryGetErrors(out _)) return BadRequest();

            var addToRoleResult = await _userManager.AddToRoleAsync(user, "User");

            if (addToRoleResult.TryGetErrors(out _)) return BadRequest();

            return Ok();
        }

        [HttpPut("signin")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(
            [FromQuery] string email,
            [FromQuery] string password)
        {
            var userName = email;

            if (EmailValidator.IsValid(email))
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null) userName = user.UserName;
            }

            var result = await _signInManager.PasswordSignInAsync(userName, password, false, false);

            if (!result.Succeeded) return BadRequest();

            return Ok();
        }

        [HttpPut("logout")]
        [Authorize(Policy = Policies.User)]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Ok();
        }

        [HttpGet("{email}")]
        [Authorize(Policy = Policies.Admin)]
        public IEnumerable<UserAccountView> GetUserByEmail(
            [FromRoute] string email)
        {
            return _iUserAccountQuery.GetUsersByEmail(email);
        }

        [HttpGet]
        [Authorize(Policy = Policies.Admin)]
        public IEnumerable<UserAccountView> GetAllUsers()
        {
            return _iUserAccountQuery.GetAll();
        }
    }
}