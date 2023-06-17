using FluentValidation;

namespace Shop.Api.Dtos.BrandDtos
{
    public class BrandPostDto
    {
        public string Name { get; set; }
    }

    public class BrandPostDtoValidator : AbstractValidator<BrandPostDto>
    {
        public BrandPostDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name cannot be empty!").MaximumLength(20).WithMessage("Maximum length should be 20!");
        }
    }
}
