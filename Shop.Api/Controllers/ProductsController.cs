using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.Helpers;
using Shop.Core.Entities;
using Shop.Core.Repositories;
using Shop.Services.Dtos.ProductDtos;
using Shop.Services.Interfaces;

namespace Shop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;

        public ProductsController(IProductRepository productRepository, IWebHostEnvironment env,IMapper mapper,IProductService productService)
        {
            _productRepository = productRepository;
            _env = env;
            _mapper = mapper;
            _productService = productService;
        }

        [HttpPost("")]
        
        public IActionResult Create([FromForm]ProductPostDto postDto)
        {
           

            return StatusCode(201, _productService.Create(postDto));
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public ActionResult Update(int id, ProductPutDto productDto) 
        {
            _productService.Edit(id, productDto);
            return StatusCode(201);

        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id) 
        {
            _productService.Delete(id);

            return NoContent();
        }

        [HttpGet("{id}")]
        public ActionResult<ProductGetByIdDto> GetById(int id) 
        {
            return Ok(_productService.GetById(id));

        }

        [HttpGet("")]
        public ActionResult<ProductGetAllDto> GetAll() 
        {
            return Ok(_productService.GetAll());
        }
    }
}
