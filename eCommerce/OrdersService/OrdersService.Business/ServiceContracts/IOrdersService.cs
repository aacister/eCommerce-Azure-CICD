using MongoDB.Driver;
using OrdersService.Business.DTO;
using OrdersService.DataAccess.Entities;

namespace OrdersService.Business.ServiceContracts
{
    public interface IOrdersService
    {
        Task<List<OrderResponse?>> GetOrders();
        Task<List<OrderResponse?>> GetOrdersByCondition(FilterDefinition<Order> filter);
        Task<OrderResponse?> GetOrderByCondition(FilterDefinition<Order> filter);
        Task<OrderResponse?> AddOrder(OrderAddRequest orderAddRequest);
        Task<OrderResponse?> UpdateOrder(OrderUpdateRequest orderUpdateRequest);
        Task<bool> DeleteOrder(Guid orderID);
    }
}
