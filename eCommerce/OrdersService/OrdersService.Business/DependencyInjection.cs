
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrdersService.Business.Mappers;
using OrdersService.Business.ServiceContracts;
using OrdersService.Business.Validators;
using os = OrdersService.Business.Services;
using StackExchange.Redis;
using OrdersService.Business.RabbitMQ;

namespace OrdersService.Business
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessDependencies(
            this IServiceCollection services,
            IConfiguration configuration
            )
        {
            services.AddValidatorsFromAssemblyContaining<OrderAddRequestValidator>();
            services.AddAutoMapper(typeof(OrderAddRequestToOrderMappingProfile).Assembly);
            services.AddScoped<IOrdersService, os.OrdersService>();
            services.AddTransient<IRabbitMQProductNameUpdateConsumer, RabbitMQProductNameUpdateConsumer>();
            services.AddHostedService<RabbitMQProductNameUpdateHostedService>();
            services.AddTransient<IRabbitMQProductDeletionConsumer, RabbitMQProductDeletionConsumer>();
            services.AddHostedService<RabbitMQProductDeletionHostedService>();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = $"{configuration["REDIS_HOST"]}:{configuration["REDIS_PORT"]}";
            });
            return services;

        }

    }
}
