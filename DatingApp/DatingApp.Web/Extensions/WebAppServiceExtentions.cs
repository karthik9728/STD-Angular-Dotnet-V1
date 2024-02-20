using DatingApp.Infrastructure.DbContexts;
using DatingApp.Web.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Web.Extensions
{
    public static class WebAppServiceExtentions
    {
        public static IServiceCollection AddWebAppServices(this IServiceCollection services, IConfiguration configuration)
        {

            #region CORS

            services.AddCors(options =>
            {
                options.AddPolicy("CustomPolicy", x =>
                {
                    x.WithOrigins("https://localhost:4200");
                    x.AllowAnyHeader();
                    x.AllowAnyMethod();
                });
            });

            #endregion


            #region Database

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            #endregion

            services.AddScoped<LogUserActivity>();

            return services;
        }
    }
}
