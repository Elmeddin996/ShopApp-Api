using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Api.Dtos.ProductDtos;
using Shop.Api.Helpers;
using Shop.Core.Entities;
using Shop.Core.Repositories;

namespace Shop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository productRepository, IWebHostEnvironment env, IMapper mapper)
        {
            _productRepository = productRepository;
            _env = env;
            _mapper = mapper;
        }

        [HttpPost("")]
        [Consumes("multipart/form-data")]
        public IActionResult Create(ProductPostDto postDto)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            if (postDto.ImageFile==null)
            {
                ModelState.AddModelError("ImageFile", "Image File is Required");
                return BadRequest(ModelState);
            }

            if (postDto.Name == null) return NoContent();



            Product product = _mapper.Map<Product>(postDto);

            _productRepository.Add(product);
            _productRepository.Commit();

            return StatusCode(201, new {product.Id, product.Name});
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public ActionResult Update(int id, ProductPutDto productDto) 
        {
            Product product = _productRepository.Get(x=>x.Id == id);

            if (product == null) return NotFound();

            product.Name = productDto.Name;
            product.BrandId = productDto.BrandId;
            product.SalePrice = productDto.SalePrice;
            product.CostPrice = productDto.CostPrice;
            product.DiscountPercent = productDto.DiscountPercent;

            string oldFileName = null;
            if (productDto.ImageFile != null)
            {
                oldFileName = product.ImageName;
                product.ImageName = FileManager.Save(_env.WebRootPath, "uploads/products", productDto.ImageFile);
            }

            _productRepository.Commit();


            if (oldFileName != null)
                FileManager.Delete(_env.WebRootPath, "uploads/products", oldFileName);

            return StatusCode(201);

        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id) 
        {
            Product product = _productRepository.Get(x=>x.Id == id);

            if (product == null) return NotFound();

            _productRepository.Remove(product);
            _productRepository.Commit();

            return NoContent();
        }

        [HttpGet("{id}")]
        public ActionResult<ProductGetByIdDto> GetById(int id) 
        {
            Product entity = _productRepository.Get(x => x.Id == id,"Brand");

            if (entity == null) return NotFound();

            ProductGetByIdDto product = _mapper.Map<ProductGetByIdDto>(entity);

            return Ok(product);

        }

        [HttpGet("")]
        public ActionResult<ProductGetAllDto> GetAll() 
        {
            var data = _mapper.Map<List<ProductGetAllDto>>(_productRepository.GetAll(x => true, "Brand"));

            if (data == null) return NotFound();


            return Ok(data);
        }
    }
}
