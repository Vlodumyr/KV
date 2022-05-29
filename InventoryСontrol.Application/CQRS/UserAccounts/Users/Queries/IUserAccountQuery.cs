using System.Collections.Generic;
using InventoryСontrol.Application.CQRS.UserAccounts.Views;

namespace InventoryСontrol.Application.CQRS.UserAccounts.Users.Queries
{
    public interface IUserAccountQuery
    {
        public IEnumerable<UserAccountView> GetUsersByEmail(string email);
        public IEnumerable<UserAccountView> GetAll();
    }
}