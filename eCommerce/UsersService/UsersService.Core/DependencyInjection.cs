using UserService.Core.ServiceContracts;
using UserService.Core.Services;
using UserService.Core.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace UserService.Core
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            //Add services to the IoC container
            services.AddTransient<IUsersService, UsersService>();
            services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();
            return services;
        }
    }
}
