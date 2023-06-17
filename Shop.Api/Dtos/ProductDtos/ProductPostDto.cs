using FluentValidation;
using Shop.Core.Atributes.ValidationAtributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Api.Dtos.ProductDtos
{
    public class ProductPostDto
    {
        public int BrandId { get; set; }
        public string Name { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CostPrice { get; set; }
        public decimal DiscountPercent { get; set; }

        [MaxFileSize(2097152)]
        [AllowedFileTypes("image/jpeg", "image/png")]
        public IFormFile ImageFile { get; set; }

    }

    public class ProductPostDtoValidator : AbstractValidator<ProductPostDto>
    {
        public ProductPostDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty!").MaximumLength(20).WithMessage("Maximum length should be 20!");
            RuleFor(x => x.SalePrice).GreaterThanOrEqualTo(x => x.CostPrice).WithMessage("Cannot be lower than the Cost price");
            RuleFor(x => x.CostPrice).GreaterThanOrEqualTo(0).WithMessage("Cannot be lower than 0");
            RuleFor(x => x.DiscountPercent).GreaterThanOrEqualTo(0).LessThanOrEqualTo(100);

            RuleFor(x => x).Custom((x, context) =>
            {
                if (x.DiscountPercent > 0)
                {
                    var price = x.SalePrice * (100 - x.DiscountPercent) / 100;
                    if (x.CostPrice > price)
                    {
                        context.AddFailure(nameof(x.DiscountPercent), "DiscountPercent is incorrect");
                    }
                }
            });

        }
    }
}
