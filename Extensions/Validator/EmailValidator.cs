using System.ComponentModel.DataAnnotations;

namespace InventoryСontrol.Api.Extensions.Validator
{
    public class EmailValidator
    {
        public static bool IsValid(string email)
        {
            return new EmailAddressAttribute().IsValid(email);
        }
    }
}