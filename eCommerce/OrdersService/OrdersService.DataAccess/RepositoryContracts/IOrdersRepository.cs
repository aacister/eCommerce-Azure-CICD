
using MongoDB.Driver;
using OrdersService.DataAccess.Entities;

namespace OrdersService.DataAccess.RepositoryContracts
{
    public interface IOrdersRepository
    {
        Task<IEnumerable<Order>> GetOrders();
        Task<IEnumerable<Order?>> GetOrdersByCondition(FilterDefinition<Order> filter);
        Task<Order?> GetOrderByCondition(FilterDefinition<Order> filter);
        Task<Order?> AddOrder(Order order);
        Task<Order?> UpdateOrder(Order order);
        Task<bool> DeleteOrder(Guid orderID);
    }
}
