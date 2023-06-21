using AutoMapper;
using Microsoft.AspNetCore.Http;
using Shop.Api.Helpers;
using Shop.Core.Entities;
using Shop.Core.Repositories;
using Shop.Services.Dtos.Common;
using Shop.Services.Dtos.ProductDtos;
using Shop.Services.Exceptions;
using Shop.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _accessor;
        private readonly IBrandRepository _brandRepository;

        public ProductService(IProductRepository productRepository,
            IMapper mapper,
            IHttpContextAccessor accessor,
            IBrandRepository brandRepository )
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _accessor = accessor;
            _brandRepository = brandRepository;
        }
        public CreateEntityDto Create(ProductPostDto dto)
        {
            List<RestExceptionError> errors = new List<RestExceptionError>();

            if (!_brandRepository.IsExist(x=>x.Id==dto.BrandId))
                errors.Add(new RestExceptionError("BrandId", "BrandId is not correct"));

            if (_productRepository.IsExist(x=>x.Name==dto.Name))
                errors.Add(new RestExceptionError("Name", "Name is already exists"));

            if (errors.Count > 0) throw new RestException(System.Net.HttpStatusCode.BadRequest, errors);

            var entity = _mapper.Map<Product>(dto);

            string rootPath = Directory.GetCurrentDirectory() + "/wwwroot";
            entity.ImageName = FileManager.Save(dto.ImageFile, rootPath, "uploads/products");

            _productRepository.Add(entity);
            _productRepository.Commit();

            return new CreateEntityDto { Id = entity.Id };

        }

        public void Delete(int id)
        {
            var entity = _productRepository.Get(x => x.Id == id);

            if (entity == null) throw new RestException(System.Net.HttpStatusCode.NotFound, "Product not found");

            _productRepository.Remove(entity);
            _productRepository.Commit();

            string rootPath = Directory.GetCurrentDirectory() + "/wwwroot";
            FileManager.Delete(rootPath, "uploads/products", entity.ImageName);
        }

        public void Edit(int id, ProductPutDto dto)
        {
            var entity = _productRepository.Get(x => x.Id == id);

            if (entity == null) throw new RestException(System.Net.HttpStatusCode.NotFound, "Product not found");

            List<RestExceptionError> errors = new List<RestExceptionError>();

            if (!_brandRepository.IsExist(x => x.Id == dto.BrandId))
                errors.Add(new RestExceptionError("BrandId", "BrandId is not correct"));

            if (dto.Name != entity.Name && _productRepository.IsExist(x => x.Name == dto.Name))
                errors.Add(new RestExceptionError("Name", "Name is already exists"));

            if (errors.Count > 0) throw new RestException(System.Net.HttpStatusCode.BadRequest, errors);


            entity.CostPrice = dto.CostPrice;
            entity.Name = dto.Name;
            entity.SalePrice = dto.SalePrice;
            entity.DiscountPercent = dto.DiscountPercent;
            entity.BrandId = dto.BrandId;

            string? removableFileName = null;
            string rootPath = Directory.GetCurrentDirectory() + "/wwwroot";

            if (dto.ImageFile != null)
            {
                removableFileName = entity.ImageName;
                entity.ImageName = FileManager.Save(dto.ImageFile, rootPath, "uploads/products");
            }

            _productRepository.Commit();

            if (removableFileName != null) FileManager.Delete(rootPath, "uploads/products", removableFileName);
        }

        public List<ProductGetAllDto> GetAll()
        {
            var entities = _productRepository.GetAll(x => true, "Brand");

            return _mapper.Map<List<ProductGetAllDto>>(entities);
        }

        public ProductGetByIdDto GetById(int id)
        {
            var entity = _productRepository.Get(x => x.Id == id, "Brand");

            if (entity == null) throw new RestException(System.Net.HttpStatusCode.NotFound, "Product not found");

            return _mapper.Map<ProductGetByIdDto>(entity); ;
        }
    }
}
