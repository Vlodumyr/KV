using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using InventoryСontrol.Application.CQRS.UserAccounts.Views;
using InventoryСontrol.Storage.Persistence;

namespace InventoryСontrol.Application.CQRS.UserAccounts.Users.Queries
{
    public sealed class UserAccountQuery : IUserAccountQuery
    {
        private static InventoryСontrolContext _context;
        private static IMapper _mapper;

        public UserAccountQuery(
            InventoryСontrolContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<UserAccountView> GetUsersByEmail(string email)
        {
            var result = _context.Users
                .Where(i => i.Email.Contains(email));
            return _mapper.Map<IEnumerable<UserAccountView>>(result);
        }

        public IEnumerable<UserAccountView> GetAll()
        {
            var result = _context.Users;
            return _mapper.Map<IEnumerable<UserAccountView>>(result);
        }
    }
}