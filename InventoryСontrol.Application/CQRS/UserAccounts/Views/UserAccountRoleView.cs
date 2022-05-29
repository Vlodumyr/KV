using System.Collections.Generic;

namespace InventoryСontrol.Application.CQRS.UserAccounts.Views
{
    public class UserAccountRoleView : UserAccountView
    {
        public List<string> Roles { get; set; }
    }
}