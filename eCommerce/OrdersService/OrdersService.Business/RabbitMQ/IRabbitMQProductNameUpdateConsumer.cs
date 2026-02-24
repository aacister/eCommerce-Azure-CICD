
namespace OrdersService.Business.RabbitMQ
{
    public interface IRabbitMQProductNameUpdateConsumer
    {
        void Consume();
        void Dispose();
    }
}
