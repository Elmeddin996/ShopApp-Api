using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.Dtos.BrandDtos;
using Shop.Core.Entities;
using Shop.Core.Repositories;

namespace Shop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandRepository _brandRepository;

        public BrandsController(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        [HttpPost("")]
        public IActionResult Create(BrandPostDto postDto)
        {
            Brand brand = new Brand
            {
                Name = postDto.Name
            };
            _brandRepository.Add(brand);
            _brandRepository.Commit();

            return StatusCode(201, new { brand.Id });
        }


        [HttpPut("{id}")]
        public IActionResult Edit(int id, BrandPutDto putDto)
        {
            Brand brand = _brandRepository.Get(x => x.Id == id);

            if (brand == null)
                return NotFound();

            brand.Name = putDto.Name;
            _brandRepository.Commit();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Brand brand = _brandRepository.Get(x => x.Id == id);

            if (brand == null)
                return NotFound();

            _brandRepository.Remove(brand);
            _brandRepository.Commit();


            return NoContent();
        }

        [HttpGet("all")]
        public ActionResult<List<BrandGetAllDto>> GetAll()
        {
            var data = _brandRepository.GetAllQueryable(x => true).Select(x => new BrandGetAllDto { Id = x.Id, Name = x.Name }).ToList();

            return Ok(data);
        }
    }
}
