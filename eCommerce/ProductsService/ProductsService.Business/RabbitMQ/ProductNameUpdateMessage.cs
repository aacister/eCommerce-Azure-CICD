
namespace ProductsService.Business.RabbitMQ;

public record ProductNameUpdateMessage(Guid ProductID, string? NewName);

