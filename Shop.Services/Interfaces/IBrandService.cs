using Shop.Services.Dtos.BrandDtos;
using Shop.Services.Dtos.Common;

namespace Shop.Services.Interfaces
{
    public interface IBrandService
    {
        CreateEntityDto Create(BrandPostDto dto);
        void Edit(int id, BrandPutDto dto);
        List<BrandGetAllDto> GetAll();
        void Delete(int id);
    }
}
