

namespace OrdersService.Business.RabbitMQ;

public interface IRabbitMQProductDeletionConsumer
{
    void Consume();
    void Dispose();
}
