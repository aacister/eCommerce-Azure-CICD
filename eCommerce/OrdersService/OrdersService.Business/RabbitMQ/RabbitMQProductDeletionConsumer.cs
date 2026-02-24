
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;


namespace OrdersService.Business.RabbitMQ
{
    public class RabbitMQProductDeletionConsumer : IRabbitMQProductDeletionConsumer
    {
        private readonly IConfiguration _configuration;
        private IChannel? _channel;
        private IConnection? _connection;
        private readonly ILogger<RabbitMQProductDeletionConsumer> _logger;
        private readonly IDistributedCache _cache;
        public RabbitMQProductDeletionConsumer(
            IConfiguration configuration,
            ILogger<RabbitMQProductDeletionConsumer> logger,
            IDistributedCache cache)
        {
            _configuration = configuration;
            _logger = logger;
            _cache = cache;

            Console.WriteLine($"RabbitMQ_HostName: {_configuration["RabbitMQ_HostName"]}");
            Console.WriteLine($"RabbitMQ_UserName: {_configuration["RabbitMQ_UserName"]}");
            Console.WriteLine($"RabbitMQ_Password: {_configuration["RabbitMQ_Password"]}");
            Console.WriteLine($"RabbitMQ_Port: {_configuration["RabbitMQ_Port"]}");

            string hostName = _configuration["RabbitMQ_HostName"]!;
            string userName = _configuration["RabbitMQ_UserName"]!;
            string password = _configuration["RabbitMQ_Password"]!;
            string port = _configuration["RabbitMQ_Port"]!;

            ConnectionFactory connectionFactory = new ConnectionFactory()
            {
                HostName = hostName,
                UserName = userName,
                Password = password,
                Port = Convert.ToInt32(port)
            };

            Task.Run(async () => await CreateChannelAsync(connectionFactory));
        }

        private async Task CreateChannelAsync(ConnectionFactory connectionFactory)
        {
            _connection = await connectionFactory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
            return;

        }
        public void Consume()
        {
            // string routingKey = "product.#";
            var headers = new Dictionary<string, object>()
      {
        { "x-match", "all" },
        { "event", "product.delete" },
        { "RowCount", 1 }
      };
            string queueName = "orders.product.delete.queue";

            if (_channel != null)
            {
                //Create exchange
                string exchangeName = _configuration["RabbitMQ_Products_Exchange"]!;
                _channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Headers, durable: true);

                //Create message queue
                _channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null); //x-message-ttl | x-max-length | x-expired 

                //Bind the message to exchange
                _channel.QueueBindAsync(queue: queueName, exchange: exchangeName, routingKey: string.Empty, arguments: headers!);

                AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(_channel);

                consumer.ReceivedAsync += async (sender, args) =>
                {
                    //Read message body
                    byte[] body = args.Body.ToArray();
                    string message = Encoding.UTF8.GetString(body);

                    if (message != null)
                    {
                        var productDeletionMessage = JsonSerializer.Deserialize<ProductDeletionMessage>(message)!;
                        _logger.LogInformation($"Product has been deleted:" +
                            $" {productDeletionMessage.ProductID}, " +
                            $"New Name: {productDeletionMessage.ProductName}");

                        await HandleProductDeletion(productDeletionMessage.ProductID);
                    }
                    await _channel.BasicAckAsync(args.DeliveryTag, false);
                };

                _channel.BasicConsumeAsync(queue: queueName,
                    consumer: consumer, autoAck: true);
            }
        }

        private async Task HandleProductDeletion(Guid productID)
        {
            string cacheKeyToWrite = $"product:{productID}";

            await _cache.RemoveAsync(cacheKeyToWrite);
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
