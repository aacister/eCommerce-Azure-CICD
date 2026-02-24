
namespace ProductsService.Business.RabbitMQ;

public record ProductDeletionMessage(Guid ProductID, string? ProductName);

