using AutoMapper;
using Shop.Core.Entities;
using Shop.Core.Repositories;
using Shop.Services.Dtos.BrandDtos;
using Shop.Services.Dtos.Common;
using Shop.Services.Exceptions;
using Shop.Services.Interfaces;


namespace Shop.Services.Implementations
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public BrandService(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }
        public CreateEntityDto Create(BrandPostDto dto)
        {
            if (_brandRepository.IsExist(x => x.Name == dto.Name))
                throw new RestException(System.Net.HttpStatusCode.BadRequest, "Name", "Name is already exist");

            var entity = _mapper.Map<Brand>(dto);

            _brandRepository.Add(entity);
            _brandRepository.Commit();

            return new CreateEntityDto { Id = entity.Id };
        }

        public void Delete(int id)
        {
            var entity = _brandRepository.Get(x => x.Id == id);

            if (entity == null) throw new RestException(System.Net.HttpStatusCode.NotFound, "Entity not found");

            _brandRepository.Remove(entity);
            _brandRepository.Commit();
        }

        public void Edit(int id, BrandPutDto dto)
        {
            var entity = _brandRepository.Get(x => x.Id == id);

            if (entity == null) throw new RestException(System.Net.HttpStatusCode.NotFound, "Entity not found");

            if (entity.Name != dto.Name && _brandRepository.IsExist(x => x.Name == dto.Name))
                throw new RestException(System.Net.HttpStatusCode.BadRequest, "Name", "Name is already exist");

            entity.Name = dto.Name;
            _brandRepository.Commit();
        }

        public List<BrandGetAllDto> GetAll()
        {
            var entities = _brandRepository.GetAll(x => true);

            return _mapper.Map<List<BrandGetAllDto>>(entities);
        }
    }
}
