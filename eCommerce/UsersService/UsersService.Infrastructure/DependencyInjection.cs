
using UserService.Core.RepositroyContracts;
using UserService.Infrastructure.DbContext;
using UserService.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace UserService.Infrastructure
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            //TODO: Add services to the IoC container
            services.AddTransient<IUsersRepository, UsersRepository>();
            services.AddTransient<DapperDbContext>();
            return services;
        }
    }
}
