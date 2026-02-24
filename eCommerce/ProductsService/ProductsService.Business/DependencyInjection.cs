using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ProductsService.Business.Mappers;
using ProductsService.Business.RabbitMQ;
using ProductsService.Business.ServiceContracts;
using ProductsService.Business.Validators;


namespace ProductsService.Business
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBusinessDependencies(
            this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ProductAddRequestToProductMappingProfile).Assembly);
            services.AddValidatorsFromAssemblyContaining<ProductAddRequestValidator>();
            services.AddScoped<IProductsService, ProductsService.Business.Services.ProductsService>();
            services.AddTransient<IRabbitMQPublisher, RabbitMQPublisher>();
            return services;
        }
    }
}
