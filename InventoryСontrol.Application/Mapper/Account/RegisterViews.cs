using AutoMapper;
using InventoryСontrol.Application.CQRS.UserAccounts.Views;
using InventoryСontrol.Domain;

namespace InventoryСontrol.Application.Mapper
{
    public sealed class RegisterViews : Profile
    {
        public RegisterViews()
        {
            CreateMap<User, UserAccountView>();
            CreateMap<User, UserAccountRoleView>()
                .ForMember(dest => dest.Roles, src => src.Ignore());
        }
    }
}