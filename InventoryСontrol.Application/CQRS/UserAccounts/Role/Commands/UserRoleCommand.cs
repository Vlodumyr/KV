using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using InventoryСontrol.Application.CQRS.UserAccounts.Views;
using InventoryСontrol.Application.Extensions;
using InventoryСontrol.Domain;
using InventoryСontrol.Storage.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InventoryСontrol.Application.CQRS.UserAccounts.Role.Commands
{
    public sealed class UserRoleCommand : IUserRoleCommand
    {
        private static UserManager<User> _userManager;
        private static InventoryСontrolContext _context;
        private static IMapper _mapper;

        public UserRoleCommand(
            UserManager<User> userManager,
            InventoryСontrolContext context,
            IMapper mapper)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserAccountRoleView> AddUserRoleAsync(Guid userId, string role)
        {
            var user = await _context.Users.FindAsync(userId.ToString());
            var result = await _userManager.AddToRoleAsync(user, role);

            if (result.TryGetErrors(out var error)) throw new ArgumentException(error);

            var userView = _mapper.Map<UserAccountRoleView>(user);

            var rolesIds = _context.UserRoles
                .Where(i => i.UserId.Equals(user.Id))
                .Select(i => i.RoleId);

            userView.Roles = await _context.Roles
                .Where(i => rolesIds.Contains(i.Id))
                .Select(i => i.Name)
                .ToListAsync();

            return userView;
        }

        public async Task<UserAccountRoleView> RemoveUserRoleAsync(Guid userId, string role)
        {
            var user = await _context.Users.FindAsync(userId.ToString());
            var result = await _userManager.RemoveFromRoleAsync(user, role);

            if (result.TryGetErrors(out var error)) throw new ArgumentException(error);

            var userView = _mapper.Map<UserAccountRoleView>(user);

            var rolesIds = _context.UserRoles
                .Where(i => i.UserId.Equals(user.Id))
                .Select(i => i.RoleId);

            userView.Roles = await _context.Roles
                .Where(i => rolesIds.Contains(i.Id))
                .Select(i => i.Name)
                .ToListAsync();

            return userView;
        }
    }
}