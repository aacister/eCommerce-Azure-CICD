using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductsService.DataAccess.Context;
using ProductsService.DataAccess.Repositories;
using ProductsService.DataAccess.RepositoryContracts;
using System.Configuration;

namespace ProductsService.DataAccess
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccessDependencies(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            string connectionStringTemplate = configuration.GetConnectionString("DefaultConnection")!;
            string connectionString = connectionStringTemplate
              .Replace("$MYSQL_HOST", Environment.GetEnvironmentVariable("MYSQL_HOST"))
              .Replace("$MYSQL_PASSWORD", Environment.GetEnvironmentVariable("MYSQL_PASSWORD"))
              .Replace("$MYSQL_DATABASE", Environment.GetEnvironmentVariable("MYSQL_DATABASE"))
              .Replace("$MYSQL_PORT", Environment.GetEnvironmentVariable("MYSQL_PORT"))
              .Replace("$MYSQL_USER", Environment.GetEnvironmentVariable("MYSQL_USER"));
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseMySQL(connectionString);
            });

            services.AddScoped<IProductsRepository, ProductsRepository>();

            return services;
        }
    }
}
