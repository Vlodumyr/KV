using AutoMapper;
using InventoryСontrol.Application.CQRS.Categories.Views;

namespace InventoryСontrol.Application.Mapper.Category
{
    public sealed class RegisterViews : Profile
    {
        public RegisterViews()
        {
            CreateMap<Domain.Category, CategoryView>();
        }
    }
}