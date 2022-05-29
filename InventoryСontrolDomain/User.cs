using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace InventoryСontrol.Domain
{
    public class User : IdentityUser
    {
        public List<PreOrder> PreOrders { get; set; }

        public static User Create(string email)
        {
            return new User
            {
                UserName = email,
                Email = email
            };
        }
    }
}