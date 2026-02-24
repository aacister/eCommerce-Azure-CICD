namespace OrdersService.Business.DTO
{
    public record OrderUpdateRequest(Guid OrderID, Guid UserID, DateTime OrderDate, List<OrderItemUpdateRequest> OrderItems)
    {
        public OrderUpdateRequest() : this(default, default, default, default)
        {
        }
    }
}
