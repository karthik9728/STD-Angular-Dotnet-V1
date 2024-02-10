using DatingApp.Application.Services;
using DatingApp.Application.Services.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.Application
{
    public static class ApplicationServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService,TokenService>();

            return services;
        }
    }
}
