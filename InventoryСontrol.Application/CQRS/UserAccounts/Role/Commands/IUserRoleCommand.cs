using System;
using System.Threading.Tasks;
using InventoryСontrol.Application.CQRS.UserAccounts.Views;

namespace InventoryСontrol.Application.CQRS.UserAccounts.Role.Commands
{
    public interface IUserRoleCommand
    {
        public Task<UserAccountRoleView> AddUserRoleAsync(Guid userId, string role);
        public Task<UserAccountRoleView> RemoveUserRoleAsync(Guid userId, string role);
    }
}