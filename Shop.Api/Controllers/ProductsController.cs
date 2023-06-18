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

        public ProductsController(IProductRepository productRepository, IWebHostEnvironment env)
        {
            _productRepository = productRepository;
            _env = env;
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



            Product product = new Product
            {
                Name = postDto.Name,
                BrandId = postDto.BrandId,
                SalePrice = postDto.SalePrice,
                CostPrice = postDto.CostPrice,
                DiscountPercent = postDto.DiscountPercent,
                ImageName = FileManager.Save(_env.WebRootPath, "uploads/products", postDto.ImageFile)
            };

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
            var data = _productRepository.Get(x => x.Id == id,"Brand");

            if (data == null) return NotFound();

            ProductGetByIdDto product = new ProductGetByIdDto
            {
                Name = data.Name,
                BrandId = data.BrandId,
                CostPrice = data.CostPrice,
                SalePrice = data.SalePrice,
                DiscountPercent = data.DiscountPercent,
                ImageName = data.ImageName
            };

            return Ok(product);

        }

        [HttpGet("")]
        public ActionResult<ProductGetAllDto> GetAll() 
        {
            var data = _productRepository.GetAllQueryable(x => true).Select(x => new ProductGetAllDto { 
                Id = x.Id,
                Name = x.Name,
                BrandId = x.BrandId,
                CostPrice = x.CostPrice,
                SalePrice = x.SalePrice,
                DiscountPercent = x.DiscountPercent,
                ImageName = x.ImageName
            }).ToList();

            if (data == null) return NotFound();


            return Ok(data);
        }
    }
}
