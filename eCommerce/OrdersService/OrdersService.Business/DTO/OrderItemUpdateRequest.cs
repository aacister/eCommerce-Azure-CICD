namespace OrdersService.Business.DTO
{
    public record OrderItemUpdateRequest(Guid ProductID, decimal UnitPrice, int Quantity)
    {
        public OrderItemUpdateRequest() : this(default, default, default)
        {
        }
    }
}
