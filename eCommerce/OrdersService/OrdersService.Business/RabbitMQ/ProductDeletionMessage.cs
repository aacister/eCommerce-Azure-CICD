
namespace OrdersService.Business.RabbitMQ;

public record ProductDeletionMessage(Guid ProductID, string? ProductName);

