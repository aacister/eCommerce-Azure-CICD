
using FluentValidation;
using ProductsService.Business.DTO;

namespace ProductsService.Business.Validators
{
    public class ProductUpdateRequestValidator : AbstractValidator<ProductUpdateRequest>
    {
        public ProductUpdateRequestValidator()
        {
            //ProductID
            RuleFor(temp => temp.ProductID)
                .NotEmpty()
                .WithMessage("Product ID can't be blank");

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
