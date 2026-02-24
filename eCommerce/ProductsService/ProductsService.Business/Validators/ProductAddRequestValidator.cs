

using FluentValidation;
using ProductsService.Business.DTO;

namespace ProductsService.Business.Validators
{
    public class ProductAddRequestValidator : AbstractValidator<ProductAddRequest>
    {
        public ProductAddRequestValidator()
        {
            
            //ProductName
            RuleFor(temp => temp.ProductName).NotEmpty()
                .WithMessage("Product Name can't be blank");

            //Category
            RuleFor(temp => temp.Category).IsInEnum().
                WithMessage("Category does not exist.");

            //UnitPrice
            RuleFor(temp => temp.UnitPrice)
                .InclusiveBetween(0, double.MaxValue)
                .WithMessage("Unit price is invalid.");

            //QuanitityInStock
            RuleFor(temp => temp.QuantityInStock)
                .InclusiveBetween(0, int.MaxValue)
                .WithMessage("Quantity in Stock is invalid.");

        }
    }
}
