using AutoMapper;
using Shop.Services.Dtos.BrandDtos;
using Shop.Services.Dtos.ProductDtos;
using Shop.Core.Entities;

namespace Shop.Api.Profiles
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Product, ProductGetByIdDto>();
            CreateMap<ProductPostDto, Product>();
            CreateMap<Product, ProductGetAllDto>()
                .ForMember(dest => dest.HasDiscount, m => m.MapFrom(s => s.DiscountPercent > 0));

            CreateMap<BrandPostDto, Brand>();
            CreateMap<Brand, BrandGetAllDto>();
            CreateMap<Brand, BrandInProductDto>();
        }
    }
}
