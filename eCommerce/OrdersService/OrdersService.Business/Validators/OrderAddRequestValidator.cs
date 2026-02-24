using FluentValidation;
using OrdersService.Business.DTO;


namespace OrdersService.Business.Validators
{
    public class OrderAddRequestValidator : AbstractValidator<OrderAddRequest>
    {
        public OrderAddRequestValidator()
        {
            //UserID
            RuleFor(temp => temp.UserID)
              .NotEmpty().WithErrorCode("User ID can't be blank");

            //OrderDate
            RuleFor(temp => temp.OrderDate)
              .NotEmpty().WithErrorCode("Order Date can't be blank");

            //OrderItems
            RuleFor(temp => temp.OrderItems)
              .NotEmpty().WithErrorCode("Order Items can't be blank");
        }
    }
}
