using AutoMapper;
using Shop.Api.Dtos.BrandDtos;
using Shop.Api.Dtos.ProductDtos;
using Shop.Core.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            CreateMap<Brand, ProductGetAllDto>();
            CreateMap<Brand, BrandInProductDto>();
        }
    }
}
